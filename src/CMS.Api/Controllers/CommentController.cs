using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Comment;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class CommentController(ICommentService commentService) : BaseController
    {
        [HttpGet]
        [CMSAuthorize]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = commentService.Get();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetSourceComments(SourceCommentDto dto)
        {
            var result = await commentService.GetSourceComments(dto);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize(CheckAccessRight = false)]
        public async Task<IActionResult> Create(CommentCreateDto dto)
        {
            var result = await commentService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<IActionResult> Update(CommentUpdateDto dto)
        {
            var result = await commentService.Update(dto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await commentService.Delete(id);
            return Ok(result);
        }
    }
}
