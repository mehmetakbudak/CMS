using CMS.Storage.Entity;
using System.Collections.Generic;

namespace CMS.Storage.Model
{
    public class AccessRightGetModel
    {
        public List<TreeModel> OperationAccessRights { get; set; }
        
        public List<TreeModel> MenuAccessRights { get; set; }
    }

    public class UserAccessRightModel
    {
        public int UserId { get; set; }
        
        public List<int> OperationUserAccessRights { get; set; }

        public List<int> MenuUserAccessRights { get; set; }
    }

    public class AccessRightModel : BaseModel
    {
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public bool IsShowMenu { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public string Endpoint { get; set; }

        public string Method { get; set; }
    }
}
