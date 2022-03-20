﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Model.Entity
{
    [Table("blogs")]
    public class Blog : BaseEntityModel
    {
        public string Url { get; set; }

        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public int NumberOfView { get; set; }

        public bool Published { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime InsertedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
        public List<SelectedBlogCategory> SelectedBlogCategories { get; set; }
    }
}
