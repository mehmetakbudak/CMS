using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    public class Article: BaseModel
    {      
        public int ArticleCategoryId { get; set; }

        public int AuthorId { get; set; }

        [StringLength(100), Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }      

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public DateTime? UpdateDate { get; set; }

        public DateTime InsertDate { get; set; }

        public ArticleCategory ArticleCategory { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }

        public ICollection<Comment> Comment { get; set; }
    }
}
