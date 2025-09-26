using CMS.Storage.Dtos.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CMS.Business.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static UserClaimsDto Parse(this ClaimsPrincipal claims)
        {
            var model = new UserClaimsDto();

            var _userId = claims.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
            if (_userId != null)
            {
                int.TryParse(_userId.Value, out int userId);
                model.UserId = userId;
            }
            var _fullEmail = claims.FindFirst(x => x.Type == ClaimTypes.Email);
            if (_fullEmail != null)
            {
                model.Email = _fullEmail.Value;
            }
            var _userType = claims.FindFirst(x => x.Type == JwtRegisteredClaimNames.Typ);
            if (_userType != null)
            {
                int.TryParse(_userType.Value, out int userType);
                model.UserType = userType;
            }
            return model;
        }
    }
}
