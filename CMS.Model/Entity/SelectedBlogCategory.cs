using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("selected_blog_categories")]
    public class SelectedBlogCategory : BaseEntityModel
    {
        public int BlogId { get; set; }

        public Blog Blog { get; set; }

        public int BlogCategoryId { get; set; }

        public BlogCategory BlogCategory { get; set; }
    }
}
