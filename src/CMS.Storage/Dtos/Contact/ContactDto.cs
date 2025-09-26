namespace CMS.Storage.Dtos.Contact
{
    public class ContactDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public int ContactCategoryId { get; set; }
        public string Message { get; set; }
    }
}
