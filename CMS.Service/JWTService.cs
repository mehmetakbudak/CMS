using CMS.Model.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMS.Service
{
    public interface IJWTService
    {
        string GenerateToken(IConfiguration configuration, User user);
    }
    public class JWTService : IJWTService
    {
        public string GenerateToken(IConfiguration configuration, User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: configuration["Jwt:Issuer"], //appsettings.json içerisinde bulunan issuer değeri
                audience: configuration["Jwt:Audience"],//appsettings.json içerisinde bulunan audince değeri
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30), // 30 gün geçerli olacak
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),//appsettings.json içerisinde bulunan signingkey değeri
                        SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
