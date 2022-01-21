using CMS.Model.Entity;
using System;

namespace CMS.Model.Model
{
    public class BlogDetailModel : BaseModel
    {
        public string CategoryUrl { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfView { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
