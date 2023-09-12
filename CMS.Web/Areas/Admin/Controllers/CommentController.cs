using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [Route("admin/comment")]
        [CMSAuthorize(RouteLevel = 2, IsView = true)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/comment/list")]
        public async Task<IActionResult> List(DataSourceLoadOptions loadOptions)
        {
            var list = await _commentService.Get();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpGet("admin/comment/{status}")]
        public async Task<IActionResult> GetAllByStatus(int status)
        {
            var list = await _commentService.GetAllByStatus(status);
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/comment")]
        public async Task<IActionResult> Put([FromBody] CommentPutModel model)
        {
            var result = await _commentService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/comment/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _commentService.Delete(id);
            return Ok(result);
        }
    }
}
