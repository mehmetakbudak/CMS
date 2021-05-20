using CMS.Model.Entity;
using System.Collections.Generic;

namespace CMS.Model.Dto
{
    public class MenuModel : BaseModel
    {
        public string Label { get; set; }
        public string To { get; set; }
        public List<MenuModel> Items { get; set; }
    }

   
}
