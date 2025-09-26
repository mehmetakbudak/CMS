using System.Collections.Generic;

namespace CMS.Storage.Dtos.Language
{
    public class DictionaryDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Items { get; set; }
    }
}
