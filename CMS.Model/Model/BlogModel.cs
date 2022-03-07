using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Published { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public List<int> BlogCategories { get; set; }
    }

    public class BlogGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
    }   

    public class BlogPostModel : BlogModel
    {

    }

    public class BlogPutModel : BlogModel
    {

    }
}
