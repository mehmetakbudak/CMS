using CMS.Storage.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using CMS.Service.Helper;
using System.Threading.Tasks;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using System.Collections.Generic;

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
        public async Task<IActionResult> Get(SourceCommentModel model)
        {
            var result = await _commentService.GetSourceComments(model);
            return Ok(result);
        }

        [HttpPost("api/comment")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Post([FromBody] CommentPostModel model)
        {
            var result = await _commentService.Post(model);
            return Ok(result);
        }

        [HttpGet("comment/user-comments")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> GetUserComments(DataSourceLoadOptions loadOptions)
        {
            var list = await _commentService.GetUserComments();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpDelete("comment/{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Delete(int id)
        {
            var user = User.Parse();
            var result = await _commentService.Delete(id, user.UserId);
            return Ok(result);
        }
    }
}
