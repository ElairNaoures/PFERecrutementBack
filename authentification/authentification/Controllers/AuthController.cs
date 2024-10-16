﻿using authentification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace authentification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public class AuthenticatedResponse
        {
            public string? Token { get; set; }
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel user)
        {
            if (user is null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "johndoe" && user.Password == "def@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                          //  new Claim(ClaimTypes.Role, "Manager")
                               new Claim(ClaimTypes.Role, "Operator")

                        };
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                         claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new AuthenticatedResponse { Token = tokenString });
            }

            return Unauthorized();
        }
        
    }
}
