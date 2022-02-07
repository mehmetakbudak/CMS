using CMS.Model.Entity;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CMS.Api.Admin
{
    [ApiController]
    [CMSAuthorize]
    [Route("api/[controller]")]
    public class AdminTodoCategoryController : ControllerBase
    {
        private readonly ITodoCategoryService todoCategoryService;

        public AdminTodoCategoryController(ITodoCategoryService todoCategoryService)
        {
            this.todoCategoryService = todoCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = todoCategoryService.GetAll().ToList();
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoCategory model)
        {
            var result = todoCategoryService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] TodoCategory model)
        {
            var result = todoCategoryService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = todoCategoryService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
