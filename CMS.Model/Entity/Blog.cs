using System;

namespace CMS.Model.Entity
{
    public class Blog : BaseModel
    {
        public int BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Count { get; set; }
        public bool Published { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Deleted { get; set; }
    }
}
