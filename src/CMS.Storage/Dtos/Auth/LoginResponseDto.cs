using System;
using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Auth
{
    public class LoginResponseDto
    {
        public int Id { get; set; }     
        public string FullName { get; set; }
        public string AccessToken { get; set; }
        public DateTime? AccessTokenExpireDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
        public UserType UserType { get; set; }
    }
}
