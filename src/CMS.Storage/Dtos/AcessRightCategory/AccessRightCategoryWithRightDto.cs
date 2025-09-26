using CMS.Storage.Dtos.AccessRight;
using System.Collections.Generic;

namespace CMS.Storage.Dtos.AcessRightCategory
{
    public class AccessRightCategoryWithRightDto : AccessRightCategoryDto
    {
        public List<AccessRightDto> Items { get; set; }
    }
}