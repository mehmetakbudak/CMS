using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [Route("admin/task")]
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();
       
        [HttpGet("admin/task/list")]
        [CMSAuthorize(RouteLevel = 3)]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _taskService.Get();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("admin/task/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskService.GetById(id);
            return Ok(result);
        }

        [HttpPost("admin/task")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Post([FromBody] TaskModel model)
        {
            var result = await _taskService.Post(model);
            return Ok(result);
        }

        [HttpPut("admin/task")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Put([FromBody] TaskModel model)
        {
            var result = await _taskService.Put(model);
            return Ok(result);
        }

        [HttpDelete("admin/task/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.Delete(id);
            return Ok(result);
        }
    }
}
