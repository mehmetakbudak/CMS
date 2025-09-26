namespace CMS.Storage.Dtos.BlogCategory
{
    public class BlogCategoryWithCountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int BlogCount { get; set; }
    }
}