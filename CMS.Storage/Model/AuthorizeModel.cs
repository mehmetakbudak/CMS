namespace CMS.Storage.Model
{
    public class AuthorizeModel
    {
        public string Endpoint { get; set; }
        public int RouteLevel { get; set; }
        public bool IsView { get; set; }
        public int UserId { get; set; }
        public string Method { get; set; }
    }
}
