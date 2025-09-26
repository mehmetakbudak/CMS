using System;

namespace CMS.Storage.Dtos.Contact
{
    public class ContactListDto
    {
        public int Id { get; set; }
        public int ContactCategoryId { get; set; }        
        public string ContactCategoryName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
