using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Entity
{
    public class Author : BaseModel
    {
        [StringLength(100), Required]
        public string NameSurname { get; set; }

        public string Resume { get; set; }

        public string File { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
