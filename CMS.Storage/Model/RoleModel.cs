using System.Collections.Generic;

namespace CMS.Storage.Model
{
    public class RoleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public List<int> AccessRightIds { get; set; }
    }
}
