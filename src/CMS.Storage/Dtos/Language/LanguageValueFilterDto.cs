using System.Collections.Generic;

namespace CMS.Storage.Dtos.Language
{
    public class LanguageValueFilterDto
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Code { get; set; }
        public List<LanguageValueDto> LanguageValues { get; set; }
    }
}
