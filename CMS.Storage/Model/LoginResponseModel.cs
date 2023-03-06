using CMS.Storage.Enum;
using System;

namespace CMS.Storage.Model
{
    public class LoginResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public UserType UserType { get; set; }
    }
}
