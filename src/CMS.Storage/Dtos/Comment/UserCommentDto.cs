using System;

namespace CMS.Storage.Dtos.Comment
{
    public class UserCommentDto
    {
        public int Id { get; set; }

        public string SourceTypeName { get; set; }

        public string Title { get; set; }

        public int SourceId { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}
