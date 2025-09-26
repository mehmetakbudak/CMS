﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    [Table("Tasks")]
    public class TaskDmo : BaseEntityModel
    {
        public int TaskCategoryId { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public int TaskStatusId { get; set; }

        public TaskStatus TaskStatus { get; set; }

        public int? AssignUserId { get; set; }

        public User AssignUser { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime InsertedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
