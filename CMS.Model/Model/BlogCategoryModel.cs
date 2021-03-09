using CMS.Model.Entity;
using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class BlogCategoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int TotalCount { get; set; }
        public List<BlogModel> Blogs { get; set; }
    }
}
