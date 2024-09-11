using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using EventsWebAPI.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EventsWebAPI.Jwt.JwtTokenProviderService
{
    public class JwtTokenProviderService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            { new Claim("ID", user.Id.ToString()),
              new Claim("Role", user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                signingCredentials: credentials,
                claims:claims,
                expires:DateTime.Now.AddHours(10)
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}
