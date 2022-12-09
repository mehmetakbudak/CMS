using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class MenuItem : BaseEntityModel
    {
        public int MenuId { get; set; }

        public Menu Menu { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
