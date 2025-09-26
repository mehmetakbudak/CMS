using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class AccessRight : BaseEntityModel
    {
        public int? AccessRightCategoryId { get; set; }

        public AccessRightCategory AccessRightCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<RoleAccessRight> RoleAccessRights { get; set; }

        public virtual List<AccessRightEndpoint> AccessRightEndpoints { get; set; }

        public virtual List<MenuItemAccessRight> MenuItemAccessRights { get; set; }
    }
}
