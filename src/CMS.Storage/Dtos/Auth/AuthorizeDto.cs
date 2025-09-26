namespace CMS.Storage.Dtos.Auth
{
    public class AuthorizeDto
    {
        public string Path { get; set; }
        public int UserId { get; set; }
        public string Method { get; set; }
    }
}
