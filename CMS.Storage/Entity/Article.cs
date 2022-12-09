using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class Article: BaseEntityModel
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
    }
}
