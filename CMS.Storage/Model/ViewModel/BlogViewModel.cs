using System;
using System.Collections.Generic;

namespace CMS.Storage.Model.ViewModel
{
    public class MostReadBlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public DateTime InsertedDate { get; set; }
    }

    public class BlogDetailViewModel
    {
        public int Id { get; set; }
        public int NumberOfView { get; set; }
        public int CommentCount { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime InsertedDate { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public List<BlogDetailCategoryModel> BlogCategories { get; set; }
    }
}
