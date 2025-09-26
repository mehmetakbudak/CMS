using System.Collections.Generic;

namespace CMS.Storage.Dtos.AccessRight
{
    public class SelectAccessRightWithCategoryDto
    {
        public int Value { get; set; }
        public string Label{ get; set; }
        public List<SelectAccessRightDto> Items { get; set; } = new List<SelectAccessRightDto>();
    }

    public class SelectAccessRightDto
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public bool Selected { get; set; }
    }
}