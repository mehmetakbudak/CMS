using CMS.Model.Enum;

namespace CMS.Model.Entity
{
    public class MailTemplate : BaseEntityModel
    {
        public TemplateType TemplateType { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
