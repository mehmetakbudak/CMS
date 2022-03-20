using CMS.Model.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("mail_templates")]
    public class MailTemplate : BaseEntityModel
    {
        public TemplateType TemplateType { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
