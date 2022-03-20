using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("web_site_parameters")]
    public class WebsiteParameter : BaseEntityModel
    {
        public int? ParentId { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }

        public bool Required { get; set; }

        public bool Visible { get; set; }
    }
}
