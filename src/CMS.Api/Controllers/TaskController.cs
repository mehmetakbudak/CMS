using CMS.Business.Attributes;
using CMS.Business.Extensions;
using CMS.Business.Services;
using CMS.Storage.Dtos.Task;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class TaskController(ITaskService taskService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = taskService.Get();
            return Ok(result);
        }

        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult GetUserTasks()
        {
            var user = User.Parse();
            var result = taskService.GetUserTasks(user.UserId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await taskService.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskDto dto)
        {
            var response = await taskService.Create(dto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskDto dto)
        {
            var response = await taskService.Update(dto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserTask([FromBody] UpdateUserTaskDto dto)
        {
            var response = await taskService.UpdateUserTask(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await taskService.Delete(id);
            return Ok(response);
        }


    }
}
