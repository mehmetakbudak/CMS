using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class ContactCategory : BaseEntityModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
