using System.Collections.Generic;

namespace CMS.Model.Entity
{
    public class ContactCategory : BaseModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
