using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class TreeModel
    {
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

    public class TreeDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<TreeDataModel> Children { get; set; }
    }
}
