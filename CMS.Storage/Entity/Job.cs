using CMS.Storage.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Storage.Entity
{
    public class Job : BaseEntityModel
    {
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        public int JobLocationId { get; set; }
        
        public JobLocation JobLocation { get; set; }

        public int InsertedUserId { get; set; }

        public User InsertedUser { get; set; }

        [StringLength(500)]
        public string Position { get; set; }

        public WorkType WorkType { get; set; }

        public string Description { get; set; }


        public DateTime InsertedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
