using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/testimonial/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _testimonialService.GetAll().ToListAsync();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/testimonial/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _testimonialService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/testimonial")]
        public async Task<IActionResult> Post(TestimonialModel model)
        {
            var result = await _testimonialService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/testimonial")]
        public async Task<IActionResult> Put(TestimonialModel model)
        {
            var result = await _testimonialService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/testimonial/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _testimonialService.Delete(id);
            return Ok(result);
        }
    }
}
