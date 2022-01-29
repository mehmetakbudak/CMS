using CMS.Model.Dto;
using CMS.Model.Helper;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api.Admin
{
    [ApiController]
    [CMSAuthorize]
    [Route("api/[controller]")]
    public class AdminTodoController : ControllerBase
    {
        private readonly ITodoService todoService;

        public AdminTodoController(ITodoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpPost("GetWithFilter")]
        public IActionResult GetWithFilter([FromBody]TodoFilterModel model)
        {
            var list = todoService.GetAll(model);
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
