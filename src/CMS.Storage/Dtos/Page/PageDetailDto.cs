using CMS.Storage.Dtos.Menu;
using System.Collections.Generic;

namespace CMS.Storage.Dtos.Page
{
    public class PageDetailDto
    {
        public int Id { get; set; }

        public int? MenuItemId { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<TreeMenuDto> MenuItems { get; set; } = new List<TreeMenuDto>();
    }
}
