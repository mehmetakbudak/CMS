using CMS.Storage.Entity;
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
        public async Task<IActionResult> Post([FromBody] TodoCategory model)
        {
            var result = await todoCategoryService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TodoCategory model)
        {
            var result = await todoCategoryService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await todoCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
