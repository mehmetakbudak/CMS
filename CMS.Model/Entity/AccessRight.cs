using CMS.Model.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class AccessRight : BaseModel
    {
        public int AccessRightCategoryId { get; set; }

        public int? ParentId { get; set; }

        public string LinkName { get; set; }

        public string LinkUrl { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public bool IsShowMenu { get; set; }

        public HttpStatusType? HttpStatusType { get; set; }

        public AccessRightCategory AccessRightCategory { get; set; }

        public ICollection<RoleAccessRight> UserGroupAccessRights { get; set; }
        public ICollection<UserAccessRight> UserAccessRights { get; set; }
    }
}
