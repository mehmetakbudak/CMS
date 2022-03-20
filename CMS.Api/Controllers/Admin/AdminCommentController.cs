using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{status}")]
        public IActionResult Get(int status)
        {
            var result = _commentService.GetAllByStatus(status);
            return Ok(result);
        }

        [HttpGet("GetDetail/{id}")]
        public IActionResult GetDetail(int id)
        {
            var result = _commentService.GetDetail(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody] CommentPutModel model)
        {
            var result = _commentService.Put(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _commentService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
