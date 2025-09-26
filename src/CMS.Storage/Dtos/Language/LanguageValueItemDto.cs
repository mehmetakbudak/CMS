using System.Collections.Generic;

namespace CMS.Storage.Dtos.Language
{
    public class LanguageValueItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public List<LanguageValueDto> Values { get; set; } = new List<LanguageValueDto>();
    }
}
