using System;

namespace CMS.Storage.Dtos.Blog
{
    public class BlogListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public DateTime InsertedDate { get; set; }
        public int CommentCount { get; set; }
        public int DisplayOrder { get; set; }
    }
}