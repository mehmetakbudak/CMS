using CMS.Model.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class AccessRight : BaseEntityModel
    {
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public bool IsShowMenu { get; set; }

        public AccessRightType Type { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<UserAccessRight> UserAccessRights { get; set; }

        public virtual ICollection<AccessRightEndpoint> AccessRightEndpoints { get; set; }
    }
}
