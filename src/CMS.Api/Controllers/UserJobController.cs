using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.UserJob;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class UserJobController(IUserJobService userJobService) : BaseController
    {
        [HttpGet]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> GetAppliedJobs()
        {
            var result = await userJobService.GetAppliedJobs();
            return Ok(result);
        }       

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Create([FromBody] UserJobCreateDto dto)
        {
            var result = await userJobService.Create(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userJobService.Delete(id);
            return Ok(result);
        }
    }
}
