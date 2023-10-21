namespace CMS.Storage.Model
{
    public class ContactModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public int ContactCategoryId { get; set; }
        public string Message { get; set; }
    }
}
