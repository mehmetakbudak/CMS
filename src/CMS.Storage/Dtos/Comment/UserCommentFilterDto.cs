using CMS.Storage.Dtos.Filter;

namespace CMS.Storage.Dtos.Comment
{
    public class UserCommentFilterDto : PagerDto
    {
        public int? Type { get; set; }
    }
}
