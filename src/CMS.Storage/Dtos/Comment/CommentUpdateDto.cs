using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Comment
{
    public class CommentUpdateDto
    {
        public int Id { get; set; }
        public CommentStatus CommentStatus { get; set; }
    }
}
