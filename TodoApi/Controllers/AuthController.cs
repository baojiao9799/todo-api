using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using TodoApi.Models;
using TodoApi.Utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TodoApi.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;
        private readonly ApplicationConfig _appConfig;

        public AuthController
        (
            IAuthRepository authRepo,
            IUserRepository userRepo,
            ApplicationConfig appConfig
        )
        {
            _authRepo = authRepo;
            _userRepo = userRepo;
            _appConfig = appConfig;
        }

        // POST: /login
        [HttpPost("/login")]
        public async Task<ActionResult<ApiResponse<CreateSessionMeta, Session>>> Login([FromBody] ApiPayload<LoginData> payload)
        {
            // Get user and check password
            LoginData loginData = payload.Data;
            var users = await _userRepo.GetAsync();
            var user = users.SingleOrDefault(user => 
                user.Username == loginData.Username
            );

            if (user == null || !PasswordUtil.IsPasswordCorrect(user, loginData.Password))
            {
                return new ApiResponse<CreateSessionMeta, Session>
                {
                    Meta = new CreateSessionMeta
                    {
                        Success = false,
                        Message = "Incorrect Username or Password"
                    }
                };
            }

            // Generate JWT
            var expiration = DateTime.UtcNow.AddMinutes(15);
            var jwtToken = JwtUtil.generateJwtToken(user, _appConfig.JwtSettings, expiration);

            var dbToken = await _authRepo.CreateAsync(new Token(jwtToken));
            
            // Set up httponly authentication cookie
            var claims = new List<Claim>
            {
                new Claim("Jwt", jwtToken),
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            };  

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme))
            );

            return new ApiResponse<CreateSessionMeta, Session>
            {
                Meta = new CreateSessionMeta
                {
                    Success = true
                },
                Data = new Session(dbToken.Id, jwtToken, user, expiration)
            };
        }
    }
}