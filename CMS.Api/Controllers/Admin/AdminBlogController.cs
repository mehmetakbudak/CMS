using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult Get(int id)
        {
            var model = blogService.GetById(id);
            return Ok(model);
        }

        [HttpPut]
        public IActionResult Put([FromBody] BlogPutModel model)
        {
            var result = blogService.Put(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
