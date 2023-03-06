using CMS.Storage.Model;
using CMS.Storage.Model.ViewModel;
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
        public async Task<IActionResult> Index()
        {
            var model = await _serviceService.GetAllActive();
            return View(model);
        }

        [Route("/service/{url}")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            ViewBag.Url = url;

            var service = await _serviceService.GetByUrl(url);

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
                Services = await _serviceService.GetAllActive()
            };

            return View(model);
        }
        #endregion

        #region APIs
        [HttpGet("api/service")]
        public async Task<IActionResult> Get()
        {
            var result = await _serviceService.GetAllActive();
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
