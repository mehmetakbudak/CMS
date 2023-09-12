using System.ComponentModel.DataAnnotations;

namespace CMS.Storage.Entity
{
    public class JobLocation : BaseEntityModel
    {
        [StringLength(500)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
