using CMS.Storage.Enum;
using Microsoft.AspNetCore.Http;

namespace CMS.Storage.Dtos.UserFile
{
    public class UserFileCreateDto
    {
        public IFormFile File { get; set; }
        public bool IsDefault { get; set; }
        public UserFileType FileType { get; set; }
    }
}
