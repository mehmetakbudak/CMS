using System;

namespace CMS.Storage.Dtos
{
    public class UserJobDto
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Position { get; set; }
        public string JobLocationName { get; set; }
        public string TenantName { get; set; }
        public string TenantImageUrl { get; set; }
        public string WorkTypeName { get; set; }
        public DateTime InsertedDate { get; set; }
    }   
}
