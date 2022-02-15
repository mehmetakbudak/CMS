﻿using CMS.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class User : BaseEntityModel
    {
        public UserStatus Status { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string HashCode { get; set; }

        public DateTime? PasswordExpireDate { get; set; }

        public UserType UserType { get; set; }

        public string Token { get; set; }

        public DateTime? TokenExpireDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public virtual ICollection<UserAccessRight> UserAccessRights { get; set; }
    }
}
