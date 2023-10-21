using CMS.Service;
using CMS.Service.Attributes;
using CMS.Service.Helper;
using CMS.Storage.Enum;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet("admin/lookup/user-types")]
        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        public IActionResult GetUserTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(UserType));
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/comment-statuses")]
        public IActionResult GetCommentStatuses()
        {
            var list = EnumHelper.GetEnumLookup(typeof(CommentStatus));
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/blog-categories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var list = await _lookupService.BlogCategories();
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/task-categories")]
        public async Task<IActionResult> GetTaskCategories()
        {
            var list = await _lookupService.TaskCategories();
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/task-statuses/{id}")]
        public async Task<IActionResult> GetTaskStatuses(int id)
        {
            var list = await _lookupService.TaskStatuses(id);
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/users")]
        public async Task<IActionResult> GetUsers()
        {
            var list = await _lookupService.Users();
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/user-statuses")]
        public IActionResult GetUserStatuses()
        {
            var list = EnumHelper.GetEnumLookup(typeof(UserStatus));
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/method-types")]
        public IActionResult GetMethodTypes()
        {
            var list = EnumHelper.GetEnumLookup(typeof(MethodType));
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/roles")]
        public async Task<IActionResult> GetRoles()
        {
            var list = await _lookupService.Roles();
            return Json(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/lookup/tags")]
        public async Task<IActionResult> GetTags()
        {
            var list = await _lookupService.Tags();
            return Json(list);
        }
    }
}
