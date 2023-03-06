using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        #region Views
        public IActionResult Index() => View();

        #endregion

        #region APIs
        [HttpGet("api/admin/comment/{status}")]
        public async Task<IActionResult> GetAllByStatus(int status)
        {
            var list = await _commentService.GetAllByStatus(status);
            return Ok(list);
        }


        #endregion
    }
}
