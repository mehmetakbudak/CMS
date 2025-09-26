﻿using System.ComponentModel;

namespace CMS.Storage.Enum
{
    public enum UserType
    {
        [Description("Süper Admin")]
        SuperAdmin = 1,
        [Description("Admin")]
        Admin,       
        [Description("Üye")]
        Member
    }
}
