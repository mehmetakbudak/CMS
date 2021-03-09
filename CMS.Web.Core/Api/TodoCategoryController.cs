using CMS.Model.Entity;
using CMS.Model.Helper;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Web.Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoCategoryController : ControllerBase
    {
        private readonly ITodoCategoryService todoCategoryService;
        public TodoCategoryController(ITodoCategoryService todoCategoryService)
        {
            this.todoCategoryService = todoCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = todoCategoryService.GetAll().ToList();
            return Ok(list);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] TodoCategory model)
        {
            var result = todoCategoryService.CreateOrUpdate(model);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoCategoryService.Delete(id);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}
