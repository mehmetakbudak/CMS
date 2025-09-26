namespace CMS.Storage.Dtos.Page
{
    public class PageDto
    {
        public int Id { get; set; }

        public int? MenuId { get; set; }

        public int? MenuItemId { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsActive { get; set; }

        public bool Published { get; set; }
    }
}
