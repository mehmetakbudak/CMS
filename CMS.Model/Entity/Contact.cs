using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class Contact : BaseModel
    {
        public string NameSurname { get; set; }
        public string EmailAddress { get; set; }
        public int ContactCategoryId { get; set; }
        public string Message { get; set; }
        public DateTime InsertDate { get; set; }
        public ContactCategory ContactCategory { get; set; }
    }
}
