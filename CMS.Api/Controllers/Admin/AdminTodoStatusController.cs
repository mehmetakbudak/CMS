using CMS.Model.Entity;
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
    public class AdminTodoStatusController : ControllerBase
    {
        private readonly ITodoStatusService todoStatusService;

        public AdminTodoStatusController(ITodoStatusService todoStatusService)
        {
            this.todoStatusService = todoStatusService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = todoStatusService.GetAll().ToList();
            return Ok(list);
        }

        [HttpGet("GetByTodoCategoryId/{todoCategoryId}")]
        public IActionResult GetByTodoCategoryId(int todoCategoryId)
        {
            var list = todoStatusService.GetByTodoCategoryId(todoCategoryId);
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoStatus model)
        {
            var result = todoStatusService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TodoStatus model)
        {
            var result = todoStatusService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoStatusService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
