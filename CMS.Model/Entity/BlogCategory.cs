using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("blog_categories")]
    public class BlogCategory : BaseEntityModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsShowHome { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
        public List<SelectedBlogCategory> SelectedBlogCategories { get; set; }
    }
}
