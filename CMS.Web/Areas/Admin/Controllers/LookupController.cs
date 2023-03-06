using CMS.Service;
using CMS.Service.Helper;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LookupController : Controller
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("api/Admin/Lookup/UserTypes")]
        public IActionResult GetUserTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(UserType));
            return Json(list);
        }

        [HttpGet("api/Admin/Lookup/CommentStatuses")]
        public IActionResult GetCommentStatuses()
        {
            var list = EnumHelper.GetEnumLookup(typeof(CommentStatus));
            return Json(list);
        }

        [HttpGet("api/Admin/Lookup/BlogCategories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var list = await _lookupService.BlogCategories();
            return Json(list);
        }

        [HttpGet("api/Admin/Lookup/TaskCategories")]
        public async Task<IActionResult> GetTaskCategories()
        {
            var list = await _lookupService.TaskCategories();
            return Json(list);
        }

        [HttpGet("api/Admin/Lookup/TaskStatuses/{id}")]
        public async Task<IActionResult> GetTaskStatuses(int id)
        {
            var list = await _lookupService.TaskStatuses(id);
            return Json(list);
        }


        [HttpGet("api/Admin/Lookup/Users")]
        public async Task<IActionResult> GetUsers()
        {
            var list = await _lookupService.Users();
            return Json(list);
        }
    }
}
