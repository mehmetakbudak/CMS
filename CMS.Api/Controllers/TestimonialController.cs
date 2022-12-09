using CMS.Model.Entity;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }

        /// <summary>
        /// Referans listesini döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Testimonial>), 200)] //OK
        public async Task<IActionResult> Get()
        {
            var list = await _testimonialService.GetAllActive();
            return Ok(list);
        }
    }
}
