using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Model.Entity
{
    public class WebsiteParameter : BaseEntityModel
    {
        public int? ParentId { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }

        public bool Required { get; set; }

        public bool Visible { get; set; }
    }
}
