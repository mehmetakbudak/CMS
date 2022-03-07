using System;
using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class BlogDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfView { get; set; }
        public DateTime InsertedDate { get; set; }
        public string ImageUrl { get; set; }
        public List<BlogDetailCategoryModel> BlogCategories{ get; set; }
    }

    public class BlogDetailCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
