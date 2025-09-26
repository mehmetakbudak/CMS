using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class TaskStatus : BaseEntityModel
    {
        public int TaskCategoryId { get; set; }

        public TaskCategory TaskCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<TaskDmo> Tasks { get; set; }
    }
}
