using CMS.Storage.Enum;

namespace CMS.Storage.Entity
{
    public class SourceTag : BaseEntityModel
    {
        public int TagId { get; set; }

        public Tag Tag { get; set; }

        public SourceType SourceType { get; set; }

        public int SourceId { get; set; }
    }
}
