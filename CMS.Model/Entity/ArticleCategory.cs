using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("article_categories")]
    public class ArticleCategory: BaseEntityModel
    {      
        public string Name { get; set; }

        public bool IsActive { get; set; }
        
        public bool Deleted { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
