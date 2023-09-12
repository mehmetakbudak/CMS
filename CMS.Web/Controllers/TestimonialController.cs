using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    public class TestimonialController : Controller
    {
        #region Constructor
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }
        #endregion

        #region Views
        public IActionResult Index() => View();
        
        #endregion

        #region APIs
        [HttpGet("api/testimonial")]
        public async Task<IActionResult> Get(int? top)
        {
            var list = await _testimonialService.GetAllActive(top);
            return Ok(list);
        }
        #endregion
    }
}
