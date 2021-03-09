using CMS.Model.Entity;
using System.Collections.Generic;

namespace CMS.Model.Dto
{
    public class AccessRightModel : BaseModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public List<AccessRightModel> Items { get; set; }
    }
}
