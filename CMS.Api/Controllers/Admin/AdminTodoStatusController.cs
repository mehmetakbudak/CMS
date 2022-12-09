using CMS.Model.Entity;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetByTodoCategoryId(int todoCategoryId)
        {
            var list = await todoStatusService.GetByTodoCategoryId(todoCategoryId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoStatus model)
        {
            var result = await todoStatusService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TodoStatus model)
        {
            var result = await todoStatusService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await todoStatusService.Delete(id);
            return Ok(result);
        }
    }
}
