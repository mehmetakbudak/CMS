using System.Collections.Generic;

namespace CMS.Model.Model
{
    public class TreeModel
    {
        public string Title { get; set; }
        public List<TreeModel> Items { get; set; }
    }

    public class TreeMenuModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string To { get; set; }
        public List<TreeMenuModel> Items { get; set; }
    }

    public class TreeDataModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string To { get; set; }
        public int? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<TreeDataModel> Items { get; set; }
    }
}
