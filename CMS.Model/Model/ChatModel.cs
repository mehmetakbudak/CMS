using CMS.Model.Entity;

namespace CMS.Model.Model
{
    public class ChatModel : BaseModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }
    }
}
