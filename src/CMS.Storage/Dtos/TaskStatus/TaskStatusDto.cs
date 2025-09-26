namespace CMS.Storage.Dtos.TaskStatus
{
    public class TaskStatusDto
    {
        public int Id { get; set; }
        public int TaskCategoryId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
    }
}
