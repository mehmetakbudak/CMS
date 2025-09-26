using CMS.Storage.Dtos.AccessRight;
using System.Collections.Generic;

namespace CMS.Storage.Dtos.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<int> AccessRightIds { get; set; }
    }
}
