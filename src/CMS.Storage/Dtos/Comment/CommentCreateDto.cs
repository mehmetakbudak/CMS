using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Comment
{
    public class CommentCreateDto
    {
        public int? ParentId { get; set; }
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public string Description { get; set; }        
    }
}
