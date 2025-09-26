using System.Text.Json.Serialization;

namespace CMS.Storage.Dtos.Menu
{
    public class MenuItemListDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ParentId { get; set; }
    }
}
