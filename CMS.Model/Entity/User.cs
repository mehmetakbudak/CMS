using CMS.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class User : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string HashCode { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public bool IsNewUser { get; set; }

        public UserType UserType { get; set; }

        public string Token { get; set; }

        public DateTime? TokenExpireDate { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; }

        public ICollection<UserAccessRight> UserAccessRights { get; set; }
    }
}
