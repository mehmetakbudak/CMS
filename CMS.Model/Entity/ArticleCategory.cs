using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMS.Model.Entity
{
    public class ArticleCategory: BaseModel
    {      
        [StringLength(100), Required]
        public string Name { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
