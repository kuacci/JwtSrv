namespace SSO
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.IdentityModel.Tokens.Jwt;

    using System.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.JsonWebTokens;
    using Microsoft.IdentityModel;
    using System.Text;
    using System.Text.Encodings;
    using SSO.Core;
    using System.Security;
    using System.Security.Claims;
    using System.Security.Cryptography;

    [Route("/account")]
    [ApiController]
    public class AccountController : Controller
    {
        public IUserService UserService { get; private set; }

        public AccountController(IUserService service)
        {
            this.UserService = service;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(LoginModel model)
        {
            var user = this.UserService.FindUser(x => x.Alias == model.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Constants.Secret);

            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Alias)
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesciptor);

            var tokenString = tokenHandler.WriteToken(token);


            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    sid = user.Alias,
                    name = user.Name,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeMilliseconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeMilliseconds()
                }
            });
        }
    }
}