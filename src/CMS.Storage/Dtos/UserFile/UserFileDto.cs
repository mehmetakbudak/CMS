using System;
using CMS.Storage.Enum;

namespace CMS.Storage.Dtos
{
    public class UserFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public UserFileType FileType { get; set; }
        public string FileTypeName { get; set; }
        public bool IsDefault { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}