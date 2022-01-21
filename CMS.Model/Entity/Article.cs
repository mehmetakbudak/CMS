using System;
using System.Collections.Generic;

namespace CMS.Model.Entity
{
    public class Article: BaseModel
    {      
        public int ArticleCategoryId { get; set; }
        
        public ArticleCategory ArticleCategory { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public string Title { get; set; }
        
        public string Content { get; set; }      
        
        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public ICollection<Comment> Comment { get; set; }
    }
}
