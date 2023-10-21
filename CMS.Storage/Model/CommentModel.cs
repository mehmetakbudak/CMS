using CMS.Storage.Enum;
using System;
using System.Collections.Generic;

namespace CMS.Storage.Model
{
    public class CommentBaseModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public SourceType SourceType { get; set; }

        public int SourceId { get; set; }

        public string Description { get; set; }
    }

    public class CommentModel : CommentBaseModel
    {
        public string Source { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public string UserFullName { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }

    public class CommentDetailModel
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

    public class CommentPostModel : CommentBaseModel { }

    public class CommentPutModel
    {
        public int Id { get; set; }

        public CommentStatus CommentStatus { get; set; }
    }

    public class CommentGetModel : CommentBaseModel
    {
        public string UserFullName { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool Expanded { get; set; }

        public List<CommentGetModel> Items { get; set; }
    }

    public class SourceCommentModel
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
    }

    public class UserCommentModel
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

    public class UserCommentFilterModel : PaginationFilterModel
    {
        public int? Type { get; set; }
    }
}
