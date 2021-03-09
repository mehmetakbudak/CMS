using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class ContactCategory : BaseModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public ICollection<Contact> Contacts{ get; set; }
    }
}
