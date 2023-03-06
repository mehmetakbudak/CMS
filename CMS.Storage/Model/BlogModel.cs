using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace CMS.Storage.Model
{
    public class BlogDetailModel
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
        public List<int> BlogCategories { get; set; }
    }

    public class BlogModel
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
    }

    public class BlogDetailCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class BlogPostModel : BlogDetailModel
    {
        public IFormFile Image { get; set; }
    }

    public class BlogPutModel : BlogDetailModel
    {
        public IFormFile Image { get; set; }
    }
}
