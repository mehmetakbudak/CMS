using CMS.Storage.Model.ViewModel;
using System.Collections.Generic;

namespace CMS.Storage.Model
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
