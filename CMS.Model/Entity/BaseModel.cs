using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Entity
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
