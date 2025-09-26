using System;
using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Comment
{
    public class CommentDetailDto
    {
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string ParentDescription { get; set; }

        public string Source { get; set; }

        public string SourceTitle { get; set; }

        public string SourceUrl { get; set; }

        public string Status { get; set; }

        public CommentStatus CommentStatus { get; set; }

        public string UserFullName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}
