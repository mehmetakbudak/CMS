using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Api.Controllers.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminBlogController : ControllerBase
    {
        private readonly IBlogService blogService;

        public AdminBlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = blogService.GetAll()
                .OrderByDescending(x => x.InsertedDate).ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var model = await blogService.GetById(id);
            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BlogPutModel model)
        {
            var result = await blogService.Put(model);
            return Ok(result);
        }
    }
}
