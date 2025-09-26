namespace CMS.Storage.Dtos.AccessRight
{
    public class AccessRightListDto
    {
        public int Id { get; set; }
        public int? AccessRightCategoryId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
