using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("api/comment")]        
        public IActionResult Get(SourceCommentModel model)
        {
            var result = _commentService.GetSourceComments(model);
            return Ok(result);
        }

        [HttpPost("api/comment")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Post([FromBody] CommentPostModel model)
        {
            var result = _commentService.Post(model);
            return Ok(result);
        }

        [HttpGet("api/comment/user-comments")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult GetUserComments(int? type)
        {
            var result = _commentService.GetUserComments(type);
            return Ok(result);
        }

        [HttpDelete("api/comment/{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public IActionResult Delete(int id)
        {
            var result = _commentService.Delete(id);
            return Ok(result);
        }
    }
}
