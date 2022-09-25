using CMS.Service;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region APIs
        [HttpGet("api/testimonial")]
        public IActionResult Get(int? top)
        {
            var list = _testimonialService.GetAllActive(top);
            return Ok(list);
        }
        #endregion
    }
}
