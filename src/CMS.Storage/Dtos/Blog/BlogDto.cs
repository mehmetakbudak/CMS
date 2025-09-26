using System.Collections.Generic;

namespace CMS.Storage.Dtos.Blog
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Published { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public string ImageUrl { get; set; }
        public List<int> BlogCategoryIds { get; set; }
        public List<int> SelectedTagIds { get; set; }
    }
}
