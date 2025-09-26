namespace CMS.Storage.Entity
{
    public class LanguageValue : BaseEntityModel
    {
        public int LanguageCodeId { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
    }
}
