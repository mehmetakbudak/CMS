using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Model.Entity
{
    public class Todo : BaseModel
    {
        public int TodoCategoryId { get; set; }
        public int TodoStatusId { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserComment { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public User User { get; set; }
        public TodoCategory TodoCategory { get; set; }
        public TodoStatus TodoStatus { get; set; }
    }
}
