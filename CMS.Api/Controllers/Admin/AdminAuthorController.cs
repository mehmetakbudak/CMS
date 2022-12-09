using CMS.Storage.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;

        public AdminAuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = authorService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthorModel model)
        {
            var result = await authorService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AuthorModel model)
        {
            var result = await authorService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await authorService.Delete(id);
            return Ok(result);
        }
    }
}
