using CMS.Storage.Entity;

namespace CMS.Storage.Dtos.AccessRight
{
    public class AccessRightDto : BaseModel
    {
        public int? AccessRightCategoryId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}