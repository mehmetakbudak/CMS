using CMS.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Model.Model
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}
