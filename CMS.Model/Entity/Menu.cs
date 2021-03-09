using CMS.Model.Enum;

namespace CMS.Model.Entity
{
    public class Menu : BaseModel
    {
        public string Name{ get; set; }
        public AccessRightCategoryType Type{ get; set; }
        public bool NotBeDeleted{ get; set; }
        public bool IsActive{ get; set; }
        public bool Deleted{ get; set; }
    }
}
