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
            var list = blogService.GetAll().OrderByDescending(x => x.InsertedDate).ToList();
            return Ok(list);
        }

        [HttpPut]
        public IActionResult Put([FromBody] BlogPutModel model)
        {
            blogService.Put(model);
            return Ok();
        }
    }
}
