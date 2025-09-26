using System.Collections.Generic;

namespace CMS.Storage.Dtos.Menu
{
    public class MenubarDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Route { get; set; }
        public int DisplayOrder { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public List<MenubarDto> Items { get; set; }
    }
}
