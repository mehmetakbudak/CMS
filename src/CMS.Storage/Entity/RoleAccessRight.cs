using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class RoleAccessRight : BaseEntityModel
    {
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }
    }
}
