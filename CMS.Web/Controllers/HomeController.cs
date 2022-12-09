using CMS.Storage.Model.ViewModel;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class HomeController : Controller
    {
        #region Constructor
        private readonly IService_Service _service_Service;
        private readonly IBlogService _blogService;
        private readonly ITestimonialService _testimonialService;
        private readonly IHomepageSliderService _homepageSliderService;
        private readonly IClientService _clientService;

        public HomeController(
            IService_Service service_Service,
            IBlogService blogService,
            ITestimonialService testimonialService,
            IHomepageSliderService homepageSliderService,
            IClientService clientService)
        {
            _service_Service = service_Service;
            _blogService = blogService;
            _testimonialService = testimonialService;
            _homepageSliderService = homepageSliderService;
            _clientService = clientService;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            var model = new HomeViewModel()
            {
                Services = _service_Service.GetAllActive(),
                Blogs = _blogService.GetBlogs(null, 10),
                Testimonials = _testimonialService.GetAllActive(),
                HomepageSliders = _homepageSliderService.GetAllActive(),
                Clients = _clientService.GetAllActive(),
            };
            return View(model);
        }
        #endregion
    }
}