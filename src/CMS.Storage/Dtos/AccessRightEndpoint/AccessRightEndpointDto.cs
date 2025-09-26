using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.AccessRightEndpoint
{
    public class AccessRightEndpointDto
    {
        public int Id { get; set; }

        public int AccessRightId { get; set; }

        public string Endpoint { get; set; }

        public MethodType Method { get; set; }
    }
}
