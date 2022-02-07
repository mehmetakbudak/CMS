using System.Collections.Generic;

namespace CMS.Model.Entity
{
    public class ArticleCategory: BaseEntityModel
    {      
        public string Name { get; set; }

        public bool IsActive { get; set; }
        
        public bool Deleted { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
