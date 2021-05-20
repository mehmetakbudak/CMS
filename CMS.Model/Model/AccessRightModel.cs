using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class AccessRightModel
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
}
