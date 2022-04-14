using Microsoft.AspNetCore.Mvc;
using TodoApi.Repositories;
using TodoApi.Models;
using TodoApi.Utils;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

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
        public async Task<ActionResult<Session>> Login(LoginData loginData)
        {
            var users = await _userRepo.GetAsync();
            var user = users.FirstOrDefault(user => 
                user.Username == loginData.Username
            );

            if (user == null || !PasswordUtil.IsPasswordCorrect(user, loginData.Password))
            {
                // Todo: return failure in metadata
                return Ok();
            }

            var jwtToken = generateJwtToken(user);

            var dbToken = await _authRepo.CreateAsync(new Token(jwtToken));
        
            return new Session(dbToken.Id, jwtToken);
        }

        private string generateJwtToken(User user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appConfig.JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString())}),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appConfig.JwtSettings.Audience,
                Issuer = _appConfig.JwtSettings.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}