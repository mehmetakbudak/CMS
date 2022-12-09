using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class AccessRightEndpoint : BaseEntityModel
    {
        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }

        public string Endpoint { get; set; }

        public string Method { get; set; }
    }
}
