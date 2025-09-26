using CMS.Storage.Entity;
using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Tag
{
    public class GetSourceTagDto
    {
        public SourceType SourceType { get; set; }
        public int? Top { get; set; }
    }
}
