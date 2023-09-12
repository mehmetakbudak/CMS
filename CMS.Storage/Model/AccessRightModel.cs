using CMS.Storage.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CMS.Storage.Model
{
    public class AccessRightGetModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Expanded { get; set; } = true;
    }

    public class UserAccessRightModel
    {
        public int UserId { get; set; }

        public List<int> UserAccessRights { get; set; }
    }

    public class AccessRightModel : BaseModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ParentId { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
