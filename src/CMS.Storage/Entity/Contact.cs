using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class Contact : BaseEntityModel
    {
        public int ContactCategoryId { get; set; }

        public ContactCategory ContactCategory { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Message { get; set; }

        public DateTime InsertedDate { get; set; }
    }
}
