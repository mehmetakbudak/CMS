using System.Collections.Generic;

namespace CMS.Storage.Entity
{ 
    public class TaskCategory : BaseEntityModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<TaskDmo> Tasks { get; set; }
    }
}
