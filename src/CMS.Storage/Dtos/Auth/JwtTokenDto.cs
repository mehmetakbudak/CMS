using System;

namespace CMS.Storage.Dtos.Auth
{
    public class JwtTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireDate { get; set; }
    }
}
