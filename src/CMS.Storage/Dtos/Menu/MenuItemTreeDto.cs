using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CMS.Storage.Dtos.Menu
{
    public class MenuItemTreeDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuItemTreeDto> Children { get; set; }
    }
}
