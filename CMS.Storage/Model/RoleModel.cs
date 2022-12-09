using CMS.Storage.Entity;
using System.ComponentModel.DataAnnotations;

namespace CMS.Storage.Model
{
    public class RoleModel
    {
        public int Id { get; set; }

        public int TotalCount { get; set; }
        
        [Required(ErrorMessage = "Role adı zorunludur.")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
