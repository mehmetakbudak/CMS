using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("GetSourceComments")]
        public IActionResult GetSourceComments([FromBody] SourceCommentModel model)
        {
            var result = _commentService.GetSourceComments(model);
            return Ok(result);
        }

        [HttpGet("GetUserComments/{type}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetUserComments(int type)
        {
            var result = _commentService.GetUserComments(type);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Post([FromBody] CommentPostModel model)
        {
            var result = _commentService.Post(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Delete(int id)
        {
            var result = _commentService.Delete(id, AuthTokenContent.Current.UserId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
