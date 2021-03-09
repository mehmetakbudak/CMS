using CMS.Model.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class AccessRightCategory : BaseModel
    {
        public string Name { get; set; }
        public AccessRightCategoryType Type { get; set; }
        public int NotBeDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public List<AccessRight> AccessRights { get; set; }
    }
}
