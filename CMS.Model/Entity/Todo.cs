using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("todos")]
    public class Todo : BaseEntityModel
    {
        public int TodoCategoryId { get; set; }

        public TodoCategory TodoCategory { get; set; }

        public int TodoStatusId { get; set; }

        public TodoStatus TodoStatus { get; set; }

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
