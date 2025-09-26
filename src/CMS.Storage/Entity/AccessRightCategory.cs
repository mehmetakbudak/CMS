using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class AccessRightCategory : BaseEntityModel
    {
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<AccessRight> AccessRights { get; set; }
    }
}
