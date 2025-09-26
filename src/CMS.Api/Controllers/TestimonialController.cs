using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Testimonial;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class TestimonialController(ITestimonialService testimonialService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = testimonialService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await testimonialService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TestimonialDto dto)
        {
            var result = await testimonialService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] TestimonialDto dto)
        {
            var result = await testimonialService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await testimonialService.Delete(id);
            return Ok(result);
        }
    }
}
