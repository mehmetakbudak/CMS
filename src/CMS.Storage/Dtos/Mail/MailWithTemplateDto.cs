using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Mail
{
    public class MailWithTemplateDto : MailDto
    {
        public TemplateType TemplateType { get; set; }
        public object Data { get; set; }
    }
}
