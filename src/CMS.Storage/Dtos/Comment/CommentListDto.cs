using System;
using CMS.Storage.Enum;
using System.Collections.Generic;

namespace CMS.Storage.Dtos.Comment
{
    public class CommentListDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
        public string Description { get; set; }
        public string UserFullName { get; set; }
        public DateTime InsertedDate { get; set; }
        public List<CommentListDto> Children { get; set; }
    }
}
