using System.Collections.Generic;

namespace CMS.Storage.Dtos.Language
{
    public class LanguageValueWithHeaderDto
    {
        public int TotalCount { get; set; }
        public List<string> Header { get; set; } = new List<string>();
        public List<LanguageValueItemDto> Items { get; set; } = new List<LanguageValueItemDto>();
    }
}
