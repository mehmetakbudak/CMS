using System.Collections.Generic;

namespace CMS.Storage.Dto
{
    public class MenuModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public List<MenuModel> Items { get; set; }
    }    
}
