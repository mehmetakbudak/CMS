using CMS.Storage.Enum;
using Microsoft.AspNetCore.Http;
using System;

namespace CMS.Storage.Model
{
    public class UserFileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public UserFileType FileType { get; set; }
        public string FileTypeName { get; set; }
        public bool IsDefault { get; set; }
        public DateTime InsertedDate { get; set; }
    }

    public class UserFilePostModel
    {
        public IFormFile File { get; set; }
        public bool IsDefault { get; set; }
        public UserFileType FileType { get; set; }
    }
}
