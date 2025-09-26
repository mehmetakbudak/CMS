using System.Collections.Generic;

namespace CMS.Storage.Dtos.AccessRight
{
    public class UserAccessRightDto
    {
        public int UserId { get; set; }
        public List<int> UserAccessRights { get; set; }
    }
}
