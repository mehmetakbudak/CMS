using CMS.Model.Model.ViewModel;
using System.Collections.Generic;

namespace CMS.Model.Model
{   
    public class BlogCategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsShowHome { get; set; }
        public bool IsActive { get; set; }
    }

    public class BlogCategoryWithCountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int BlogCount { get; set; }
    }
}
