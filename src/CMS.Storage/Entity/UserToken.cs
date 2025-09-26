using System;

namespace CMS.Storage.Entity
{
    public class UserToken : BaseEntityModel
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpireDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireDate { get; set; }
    }
}
