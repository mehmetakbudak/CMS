namespace CMS.Storage.Dtos.Language
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
