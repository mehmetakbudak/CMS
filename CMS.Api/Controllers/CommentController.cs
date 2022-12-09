using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using CMS.Service.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [ProducesResponseType(typeof(List<CommentGetModel>), 200)] //OK
        public async Task<IActionResult> GetSourceComments([FromBody] SourceCommentModel model)
        {
            var result = await _commentService.GetSourceComments(model);
            return Ok(result);
        }

        [HttpGet("GetUserComments/{type?}")]
        [CMSAuthorize(CheckAccessRight = false)]
        [ProducesResponseType(typeof(List<UserCommentModel>), 200)] //OK
        public async Task<IActionResult> GetUserComments([FromQuery] UserCommentFilterModel model)
        {
            var list = await _commentService.GetUserComments(model.Type);
            var pagedReponse = PaginationHelper.CreatePagedReponse<UserCommentModel>(list, model);
            return Ok(pagedReponse);
        }

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Post([FromBody] CommentPostModel model)
        {
            var result = await _commentService.Post(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _commentService.Delete(id);
            return Ok(result);
        }
    }
}
