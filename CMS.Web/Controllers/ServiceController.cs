using CMS.Model.Model;
using CMS.Model.Model.ViewModel;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class ServiceController : Controller
    {
        #region Constructor
        public readonly IService_Service _serviceService;

        public ServiceController(IService_Service serviceService)
        {
            _serviceService = serviceService;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            var model = _serviceService.GetAllActive();
            return View(model);
        }

        [Route("/service/{url}")]
        public IActionResult GetByUrl(string url)
        {
            ViewBag.Url = url;

            var service = _serviceService.GetByUrl(url);

            var model = new ServiceDetailViewModel
            {
                Detail = new ServiceModel
                {
                    Id = service.Id,
                    Name = service.Name,
                    ImageUrl = service.ImageUrl,
                    Url = service.Url,
                    Content = service.Content
                },
                Services = _serviceService.GetAllActive()
            };

            return View(model);
        }
        #endregion

        #region APIs
        [HttpGet("api/service")]
        public IActionResult Get()
        {
            var result = _serviceService.GetAllActive();
            return Ok(result);
        }

        [HttpGet("api/service/{url}")]
        public IActionResult GetServiceByUrl(string url)
        {
            var result = _serviceService.GetByUrl(url);
            return Ok(result);
        }
        #endregion
    }
}
