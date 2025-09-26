using CMS.Storage.Enum;

namespace CMS.Storage.Entity
{
    public class AccessRightEndpoint : BaseEntityModel
    {
        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }

        public string Endpoint { get; set; }

        public MethodType Method { get; set; }
    }
}
