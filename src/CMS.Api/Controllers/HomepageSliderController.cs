using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.HomepageSlider;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class HomepageSliderController(IHomepageSliderService homepageSliderService) : BaseController
    {
        [HttpGet]
        [CMSAuthorize]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = homepageSliderService.Get();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await homepageSliderService.GetAllActive();
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await homepageSliderService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<IActionResult> Create([FromForm] HomepageSliderDto dto)
        {
            var result = await homepageSliderService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update([FromForm] HomepageSliderDto dto)
        {
            var result = await homepageSliderService.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await homepageSliderService.Delete(id);
            return Ok(result);
        }
    }
}
