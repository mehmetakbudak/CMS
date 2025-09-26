using CMS.Storage.Dtos.Filter;
using System;

namespace CMS.Storage.Dtos.Task
{
    public class TaskFilterDto : PagerDto
    {
        public int? TaskCategoryId { get; set; }
        public string Title { get; set; }
        public int? AssignUserId { get; set; }
        public int? TaskStatusId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
