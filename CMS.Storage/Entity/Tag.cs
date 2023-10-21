using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class Tag : BaseEntityModel
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<SourceTag> SourceTags { get; set; }
    }
}
