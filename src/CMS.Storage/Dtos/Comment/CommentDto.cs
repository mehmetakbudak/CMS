using System;
using CMS.Storage.Enum;

namespace CMS.Storage.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string UserFullName { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}