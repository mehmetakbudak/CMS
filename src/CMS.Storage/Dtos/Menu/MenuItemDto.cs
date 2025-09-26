using CMS.Storage.Enum;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMS.Storage.Dtos.Menu
{
    public class MenuItemDto
    {
        public int Id { get; set; }

        public int MenuId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ParentId { get; set; }

        public List<int> AccessRightIds { get; set; } = new List<int>();
    }
}
