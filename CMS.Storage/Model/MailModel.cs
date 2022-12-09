using CMS.Storage.Enum;

namespace CMS.Storage.Model
{
    public class MailModel
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class MailWithTemplateModel : MailModel
    {

        public TemplateType TemplateType { get; set; }

        public object Data { get; set; }
    }
}
