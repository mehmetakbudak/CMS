﻿using CMS.Storage.Enum;
using System;

namespace CMS.Storage.Entity
{
    public class Comment : BaseEntityModel
    {
        public int? ParentId { get; set; }

        public SourceType SourceType { get; set; }

        public int SourceId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public CommentStatus Status { get; set; }

        public string Description { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool Deleted { get; set; }

    }
}
