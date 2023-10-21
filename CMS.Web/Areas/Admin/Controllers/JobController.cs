using CMS.Service;
using CMS.Service.Attributes;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [Route("admin/job")]
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/job/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _jobService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

    }
}
