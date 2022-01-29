namespace CMS.Model.Entity
{
    public class SelectedBlogCategory : BaseModel
    {
        public int BlogId { get; set; }

        public Blog Blog { get; set; }

        public int BlogCategoryId { get; set; }

        public BlogCategory BlogCategory { get; set; }
    }
}
