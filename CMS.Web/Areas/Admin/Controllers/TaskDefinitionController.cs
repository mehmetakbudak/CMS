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
    public class TaskDefinitionController : Controller
    {
        private readonly ITaskCategoryService _taskCategoryService;
        private readonly ITaskStatusService _taskStatusService;
        public TaskDefinitionController(
            ITaskCategoryService taskCategoryService,
            ITaskStatusService taskStatusService)
        {
            _taskCategoryService = taskCategoryService;
            _taskStatusService = taskStatusService;
        }

        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/task-category/list")]
        public async Task<IActionResult> GetTaskCategory(DataSourceLoadOptions loadOptions)
        {
            var list = await _taskCategoryService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/task-category/{id}")]
        public async Task<IActionResult> GetTaskCategory(int id)
        {
            var result = await _taskCategoryService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/task-category")]
        public async Task<IActionResult> PostTaskCategory([FromBody] TaskCategoryModel model)
        {
            var result = await _taskCategoryService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/task-category")]
        public async Task<IActionResult> PutTaskCategory([FromBody] TaskCategoryModel model)
        {
            var result = await _taskCategoryService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/task-category/{id}")]
        public async Task<IActionResult> DeleteTaskCategory(int id)
        {
            var result = await _taskCategoryService.Delete(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/task-status/list")]
        public async Task<IActionResult> GetTaskStatus(DataSourceLoadOptions loadOptions)
        {
            var list = await _taskStatusService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/task-status/{id}")]
        public async Task<IActionResult> GetTaskStatus(int id)
        {
            var result = await _taskStatusService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/task-status")]
        public async Task<IActionResult> Post([FromBody] TaskStatusModel model)
        {
            var result = await _taskStatusService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/task-status")]
        public async Task<IActionResult> Put([FromBody] TaskStatusModel model)
        {
            var result = await _taskStatusService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/task-status/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskStatusService.Delete(id);
            return Ok(result);
        }
    }
}
