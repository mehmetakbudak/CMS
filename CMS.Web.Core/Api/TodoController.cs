using CMS.Model.Dto;
using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Web.Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService todoService;
        public TodoController(ITodoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = todoService.GetAll().ToList();
            return Ok(list);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] TodoModel model)
        {
            var result = todoService.CreateOrUpdate(model);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoService.Delete(id);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}
