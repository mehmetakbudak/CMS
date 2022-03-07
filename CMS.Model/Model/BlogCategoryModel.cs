using CMS.Model.Entity;
using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class BlogCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<BlogGetModel> Blogs { get; set; }
    }

    public class BlogCategoryDtoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsShowHome { get; set; }
        public bool IsActive { get; set; }        
    }
}
