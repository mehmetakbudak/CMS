using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class Page : BaseEntityModel
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool Published { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
