using CMS.Model.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class Menu : BaseEntityModel
    {
        public string Name { get; set; }

        public MenuType Type { get; set; }

        public bool IsDeletable { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
