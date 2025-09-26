using System;

namespace CMS.Storage.Dtos.Blog
{
    public class BlogGridDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public int DisplayOrder { get; set; }
        public int NumberOfView { get; set; }
        public bool IsActive { get; set; }
        public bool Published { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
