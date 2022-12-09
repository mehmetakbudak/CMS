using CMS.Storage.Dto;
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
    public class AdminTodoController : ControllerBase
    {
        private readonly ITodoService todoService;

        public AdminTodoController(ITodoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpPost("GetWithFilter")]
        public async Task<IActionResult> GetWithFilter([FromBody] TodoFilterModel model)
        {
            var list = await todoService.GetAll(model);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TodoModel model)
        {
            var result = await todoService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TodoModel model)
        {
            var result = await todoService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await todoService.Delete(id);
            return Ok(result);
        }
    }
}
