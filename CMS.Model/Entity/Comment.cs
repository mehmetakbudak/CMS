using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Entity
{
    public class Comment: BaseModel
    {       
        public int ArticleId { get; set; }

        public int UserId { get; set; }

        public CommentStatus CommentStatus { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        public DateTime InsertDate { get; set; }
        public bool Deleted { get; set; }

        public virtual Article Article { get; set; }

        public virtual User User { get; set; }
    }

    public enum CommentStatus
    {
        WaitingforApproval,
        Approved,
        Rejected
    }
}
