using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Job;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class JobController(IJobService jobService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = jobService.Get();
            return Ok(result);
        }

        [HttpGet]        
        public async Task<IActionResult> GetAllActive()
        {
            var result = await jobService.GetAllActive();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailById(int id)
        {
            var result = await jobService.GetDetailById(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await jobService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<IActionResult> Create(JobDto dto)
        {
            var result = await jobService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update(JobDto dto)
        {
            var result = await jobService.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await jobService.Delete(id);
            return Ok(result);
        }
    }
}
