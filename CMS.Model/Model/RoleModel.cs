using CMS.Model.Entity;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Model
{
    public class RoleModel : BaseModel
    {
        public int TotalCount { get; set; }
        [Required(ErrorMessage = "Role adı zorunludur.")]
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
