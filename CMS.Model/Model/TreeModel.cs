using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class TreeModel
    {
        public int Key { get; set; }
        public string Label { get; set; }
        public List<TreeModel> Children { get; set; }
    }

    public class TreeMenuModel
    {
        public int Key { get; set; }
        public string Label { get; set; }
        public string To { get; set; }
        public List<TreeMenuModel> Children { get; set; }
    }
}
