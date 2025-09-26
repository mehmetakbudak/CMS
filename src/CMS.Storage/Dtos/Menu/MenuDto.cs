using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Menu
{
    public class MenuDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public MenuType Type { get; set; }

        public bool IsDeletable { get; set; }

        public bool IsActive { get; set; }
    }
}