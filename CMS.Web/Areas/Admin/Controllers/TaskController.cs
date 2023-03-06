using CMS.Service;
using CMS.Storage.Dto;
using Microsoft.AspNetCore.Mvc;

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

        #region View
        [Route("admin/task")]
        public IActionResult Index() => View();

        [Route("admin/task/add")]
        public IActionResult Add()
        {
            ViewBag.Id = 0;
            return View("CreateOrUpdate");
        }

        [Route("admin/task/edit/{id}")]
        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View("CreateOrUpdate");
        }
        #endregion

        #region API
        [HttpPost("api/admin/task/getbyfilter")]
        public IActionResult GetByFilter([FromBody] TaskFilterModel model)
        {
            var list = _taskService.GetAll(model);
            return Ok(list);
        }

        [HttpGet("api/admin/task/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskService.GetById(id);
            return Ok(result);
        }

        [HttpPost("api/admin/task")]
        public async Task<IActionResult> Post([FromBody] TaskModel model)
        {
            var result = await _taskService.Post(model);
            return Ok(result);
        }

        [HttpPut("api/admin/task")]
        public async Task<IActionResult> Put([FromBody] TaskModel model)
        {
            var result = await _taskService.Put(model);
            return Ok(result);
        }

        [HttpDelete("api/admin/task/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.Delete(id);
            return Ok(result);
        }
        #endregion
    }
}
