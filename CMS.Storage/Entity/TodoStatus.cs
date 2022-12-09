using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class TodoStatus : BaseEntityModel
    {
        public int TodoCategoryId { get; set; }

        public TodoCategory TodoCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<TaskDmo> Todos { get; set; }
    }
}
