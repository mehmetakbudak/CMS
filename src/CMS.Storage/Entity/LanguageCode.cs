using System.Collections.Generic;

namespace CMS.Storage.Entity
{
    public class LanguageCode : BaseEntityModel
    {
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public virtual ICollection<LanguageValue> LanguageValues { get; set; }
    }
}