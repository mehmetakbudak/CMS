using CMS.Storage.Dtos.Filter;

namespace CMS.Storage.Dtos.Blog
{
    public class BlogListFilterDto : PagerDto
    {
        public string SearchText { get; set; }
    }
}
