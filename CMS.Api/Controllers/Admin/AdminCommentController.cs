using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get(int status)
        {
            var result = await _commentService.GetAllByStatus(status);
            return Ok(result);
        }

        [HttpGet("GetDetail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var result = await _commentService.GetDetail(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CommentPutModel model)
        {
            var result = await _commentService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _commentService.Delete(id);
            return Ok(result);
        }
    }
}
