using CMS.Storage.Enum;

namespace CMS.Storage.Dtos.Job
{
    public class JobDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int JobLocationId { get; set; }
        public string Position { get; set; }
        public WorkType WorkType { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
