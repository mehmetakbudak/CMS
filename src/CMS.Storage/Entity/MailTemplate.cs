using CMS.Storage.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class MailTemplate : BaseEntityModel
    {
        public TemplateType TemplateType { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}
