using CMS.Model.Enum;
using System;

namespace CMS.Model.Entity
{
    public class Comment : BaseEntityModel
    {
        public SourceType SourceType { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public CommentStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool Deleted { get; set; }

    }
}
