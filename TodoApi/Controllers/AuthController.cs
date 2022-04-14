using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using TodoApi.Models;
using TodoApi.Utils;

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
            LoginData loginData = payload.Data;
            var users = await _userRepo.GetAsync();
            var user = users.FirstOrDefault(user => 
                user.Username == loginData.Username
            );

            var response = new ApiResponse<CreateSessionMeta, Session>();

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
            var expiration = DateTime.UtcNow.AddMinutes(15);
            var jwtToken = JwtUtil.generateJwtToken(user, _appConfig.JwtSettings, expiration);

            var dbToken = await _authRepo.CreateAsync(new Token(jwtToken));

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