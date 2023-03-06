using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    [Table("TaskStatuses")]
    public class TaskStatusDmo : BaseEntityModel
    {
        public int TaskCategoryId { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<TaskDmo> Tasks { get; set; }
    }
}
