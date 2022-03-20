using CMS.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class CommentBaseModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Yorum tipi zorunludur.")]
        public SourceType SourceType { get; set; }

        [Required(ErrorMessage = "Id bilgisi zorunludur.")]
        public int SourceId { get; set; }

        [Required(ErrorMessage = "Yorum zorunludur.")]
        public string Description { get; set; }
    }

    public class CommentModel : CommentBaseModel
    {
        public string Source { get; set; }

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
        public int Key { get; set; }

        public string UserFullName { get; set; }

        public DateTime InsertedDate { get; set; }

        public List<CommentGetModel> Children { get; set; }
    }

    public class SourceCommentModel
    {
        public SourceType SourceType { get; set; }
        public int SourceId { get; set; }
    }

    public class UserCommentModel
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}
