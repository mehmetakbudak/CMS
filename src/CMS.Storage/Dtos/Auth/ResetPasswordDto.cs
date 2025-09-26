namespace CMS.Storage.Dtos.Auth
{
    public class ResetPasswordDto
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }
}
