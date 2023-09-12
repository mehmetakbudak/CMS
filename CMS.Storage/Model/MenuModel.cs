using CMS.Storage.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CMS.Storage.Model
{
    public class MenuModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public List<MenuModel> Items { get; set; }
    }

    public class MenuItemModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public MenuType MenuType { get; set; }

        public int DisplayOrder { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ParentId { get; set; }

        public List<int> AccessRightIds { get; set; }
    }

    public class MenuItemGetModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentId { get; set; }

        public bool Expanded { get; set; }
    }

}
