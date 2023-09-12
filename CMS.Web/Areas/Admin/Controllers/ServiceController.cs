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
    public class ServiceController : Controller
    {
        private readonly IService_Service _serviceService;

        public ServiceController(IService_Service serviceService)
        {
            _serviceService = serviceService;
        }

        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/service/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _serviceService.GetAll().ToListAsync();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/service/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _serviceService.GetById(id);
            return Ok(result);
        }

        [HttpPost("admin/service")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Post(ServiceModel model)
        {
            var result = await _serviceService.Post(model);
            return Ok(result);
        }

        [HttpPut("admin/service")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Put(ServiceModel model)
        {
            var result = await _serviceService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/service/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceService.Delete(id);
            return Ok(result);
        }
    }
}
