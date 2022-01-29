using System;

namespace CMS.Model.Model
{
    public class BlogDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfView { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
