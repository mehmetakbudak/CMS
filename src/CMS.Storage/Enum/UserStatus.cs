using System.ComponentModel;

namespace CMS.Storage.Enum
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
