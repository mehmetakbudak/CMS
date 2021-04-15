using CMS.Model.Entity;
using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoStatusController : ControllerBase
    {
        private readonly ITodoStatusService todoStatusService;
        public TodoStatusController(ITodoStatusService todoStatusService)
        {
            this.todoStatusService = todoStatusService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = todoStatusService.GetAll().ToList();
            return Ok(list);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] TodoStatus model)
        {
            var result = todoStatusService.CreateOrUpdate(model);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoStatusService.Delete(id);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}
