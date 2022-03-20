using System.ComponentModel;

namespace CMS.Model.Enum
{
    public enum UserStatus
    {
        [Description("Aktif")]
        Active = 1,
        [Description("Email Doğrulanmamış")]
        EmailNotVerified,
        [Description("Şifre Belirlenmemiş")]
        NotSetPassword
    }
}
