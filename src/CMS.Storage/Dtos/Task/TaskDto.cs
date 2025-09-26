namespace CMS.Storage.Dtos.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int TaskCategoryId { get; set; }
        public int TaskStatusId { get; set; }
        public int? AssignUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
