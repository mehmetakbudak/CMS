using CMS.Model.Enum;
using System.Collections.Generic;

namespace CMS.Model.Entity
{
    public class Menu : BaseModel
    {
        public string Name { get; set; }

        public MenuType Type { get; set; }

        public bool Deletable { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<MenuItems> MenuItems { get; set; }
    }
}
