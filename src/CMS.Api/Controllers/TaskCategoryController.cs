using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.TaskCategory;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class TaskCategoryController(ITaskCategoryService taskCategoryService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = taskCategoryService.Get();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = taskCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCategoryDto dto)
        {
            var result = await taskCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TaskCategoryDto dto)
        {
            var result = await taskCategoryService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await taskCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
