namespace CMS.Model.Model
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
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

    public class BlogBaseModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfView { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class BlogPostModel : BlogBaseModel
    {
    }

    public class BlogPutModel : BlogBaseModel
    {
        public int Id { get; set; }
    }
}
