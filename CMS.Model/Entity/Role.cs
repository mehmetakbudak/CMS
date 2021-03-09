using System.Collections.Generic;

namespace CMS.Model.Entity
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public ICollection<RoleAccessRight> RoleAccessRights { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
