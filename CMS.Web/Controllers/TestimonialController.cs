using CMS.Service;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
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

        [HttpGet("testimonial/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions, int top)
        {
            var list = await _testimonialService.GetAllActive();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }
    }
}
