using CMS.Model.Entity;
using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class BlogCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int TotalCount { get; set; }
        public List<BlogModel> Blogs { get; set; }
    }
}
