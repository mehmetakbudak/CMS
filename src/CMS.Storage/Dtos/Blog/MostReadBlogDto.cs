using System;

namespace CMS.Storage.Dtos.Blog
{
    public class MostReadBlogDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
