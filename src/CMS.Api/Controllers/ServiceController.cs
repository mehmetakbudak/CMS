using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class ServiceController(IService_Service service) : BaseController
    {
        [HttpGet]
        [CMSAuthorize]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = service.Get();
            return Ok(result);
        }

        [HttpGet("{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await service.GetByUrl(url);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await service.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<IActionResult> Create([FromForm] ServiceDto dto)
        {
            var result = await service.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update([FromForm] ServiceDto dto)
        {
            var result = await service.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.Delete(id);
            return Ok(result);
        }
    }
}
