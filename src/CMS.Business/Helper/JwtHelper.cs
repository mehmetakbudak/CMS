using CMS.Business.Exceptions;
using CMS.Business.Infrastructure;
using CMS.Storage.Dtos.Auth;
using CMS.Storage.Entity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CMS.Business.Helper
{
    public interface IJwtHelper
    {
        JwtTokenDto GenerateJwtToken(User user);
        JwtSecurityToken ValidateToken(string token);
        string GenerateRefreshToken();
    }

    public class JwtHelper : IJwtHelper
    {
        public JwtTokenDto GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Global.Secret);
            var accessTokenExpiration = DateTime.Now.AddMinutes(Global.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(Global.RefreshTokenExpiration);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                     new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                     new Claim(JwtRegisteredClaimNames.Typ, ((int)user.UserType).ToString()),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = accessTokenExpiration,
                Issuer = Global.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var model = new JwtTokenDto
            {
                AccessToken = token,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpireDate = refreshTokenExpiration,
                AccessTokenExpireDate = accessTokenExpiration
            };
            return model;
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            try
            {
                JwtSecurityToken jwtToken = null;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Global.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (validatedToken != null)
                {
                    jwtToken = (JwtSecurityToken)validatedToken;
                }
                else
                {
                    throw new BadRequestException("Token doğrulanamadı.");
                }
                return jwtToken;
            }
            catch (Exception)
            {
                throw new UnAuthorizedException("Token doğrulanamadı.");
            }
        }

        public string GenerateRefreshToken()
        {
            var numberByte = new Byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
