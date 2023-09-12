namespace CMS.Storage.Model
{
    public class AccessRightEndpointGetModel
    {
        public int Id { get; set; }
        public int AccessRightId { get; set; }

        public string Endpoint { get; set; }

        public string MethodName { get; set; }

        public int RouteLevel { get; set; }
    }
}
