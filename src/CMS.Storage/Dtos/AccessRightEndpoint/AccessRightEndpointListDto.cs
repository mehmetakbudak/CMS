namespace CMS.Storage.Dtos.AccessRightEndpoint
{
    public class AccessRightEndpointListDto
    {
        public int Id { get; set; }
        public int AccessRightId { get; set; }
        public string Endpoint { get; set; }
        public string MethodName { get; set; }
    }
}