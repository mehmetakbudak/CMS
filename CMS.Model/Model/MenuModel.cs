using System.Collections.Generic;

namespace CMS.Model.Dto
{
    public class MenuModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public List<MenuModel> Items { get; set; }
    }

    public class MenubarModel
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string To { get; set; }

        public List<MenubarModel> Items { get; set; }
    }
}
