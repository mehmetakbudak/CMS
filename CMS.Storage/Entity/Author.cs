using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Storage.Entity
{
    public class Author : BaseEntityModel
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Resume { get; set; }

        public string ImageUrl { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime InsertedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
