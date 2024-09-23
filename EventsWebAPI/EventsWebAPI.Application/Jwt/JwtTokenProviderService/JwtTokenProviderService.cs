using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using EventsWebAPI.Core.Models;

namespace EventsWebAPI.Application.Jwt.JwtTokenProviderService
{
    public class JwtTokenProviderService
    {
        public string GenerateToken(User user)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("spinnersarebouttoinvadetheworld")), SecurityAlgorithms.HmacSha256);

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
