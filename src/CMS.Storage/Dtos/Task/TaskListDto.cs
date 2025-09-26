using Microsoft.AspNetCore.Mvc;
using System;

namespace CMS.Storage.Dtos.Task
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public int TaskCategoryId { get; set; }
        public int TaskStatusId { get; set; }
        public int? AssignUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string UserNameSurname { get; set; }
        public string TaskStatusName { get; set; }
        public string TaskCategoryName { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
