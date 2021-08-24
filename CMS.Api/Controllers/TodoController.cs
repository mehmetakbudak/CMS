using CMS.Model.Dto;
using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api
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

        [HttpPost]
        public IActionResult Post([FromBody] TodoModel model)
        {
            var result = todoService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TodoModel model)
        {
            var result = todoService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
