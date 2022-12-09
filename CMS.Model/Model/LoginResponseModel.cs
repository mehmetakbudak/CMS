using CMS.Model.Enum;
using System;

namespace CMS.Model.Model
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public UserType UserType { get; set; }
    }
}
