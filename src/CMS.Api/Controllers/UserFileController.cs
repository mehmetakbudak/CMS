using CMS.Business.Attributes;
using CMS.Business.Extensions;
using CMS.Business.Services;
using CMS.Storage.Dtos.UserFile;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class UserFileController(IUserFileService userFileService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetUserFiles()
        {
            var user = User.Parse();
            var result = userFileService.GetUserFiles(user.UserId);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Create([FromForm] UserFileCreateDto dto)
        {
            var result = await userFileService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> SetDefault(int id)
        {
            var result = await userFileService.SetDefault(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userFileService.Delete(id);
            return Ok(result);
        }
    }
}
