using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Entity
{
    public class BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
    }

    public class BaseModel
    {
        public int Id { get; set; }
    }

    public class BaseNullableModel
    {
        public int? Id { get; set; }
    }
}
