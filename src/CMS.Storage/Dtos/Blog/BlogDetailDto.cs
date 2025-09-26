using System;
using System.Collections.Generic;

namespace CMS.Storage.Dtos.Blog
{
    public class BlogDetailDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int NumberOfView { get; set; }
        public DateTime InsertedDate { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public int CommentCount { get; set; }
        public string Url { get; set; }
        public List<BlogDetailCategoryDto> BlogCategories { get; set; }
        public List<BlogDetailTagDto> BlogTags { get; set; }
    }
}
