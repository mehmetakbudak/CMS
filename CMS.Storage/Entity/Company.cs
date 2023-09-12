using System.ComponentModel.DataAnnotations;

namespace CMS.Storage.Entity
{
    public class Company : BaseEntityModel
    {
        [StringLength(1000)]
        public string Name { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string WebSiteUrl { get; set; }

        [StringLength(500)]
        public string LinkedUrl { get; set; }

        [StringLength(500)]
        public string TwitterUrl { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
