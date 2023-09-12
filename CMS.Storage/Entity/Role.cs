using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class Role : BaseEntityModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<RoleAccessRight> RoleAccessRights { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
