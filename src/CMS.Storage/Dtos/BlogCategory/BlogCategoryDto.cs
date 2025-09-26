namespace CMS.Storage.Dtos.BlogCategory
{
    public class BlogCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsShowHome { get; set; }
        public bool IsActive { get; set; }
    }
}