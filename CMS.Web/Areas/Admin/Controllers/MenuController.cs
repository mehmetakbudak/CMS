using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [CMSAuthorize(IsView = true, RouteLevel = 2)]
        public IActionResult Index() => View();

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/menu/frontend-menu")]
        public async Task<IActionResult> GetFrontendMenu()
        {
            var list = await _menuService.GetFrontendMenu();
            return Ok(list);
        }

        [CMSAuthorize(RouteLevel = 3)]
        [HttpGet("admin/menu/admin-menu")]
        public async Task<IActionResult> GetAdminMenu()
        {
            var list = await _menuService.GetAdminMenu();
            return Ok(list);
        }

        [CMSAuthorize(CheckAccessRight = false, RouteLevel = 3)]
        [HttpGet("admin/menu/user-admin-menu")]
        public async Task<IActionResult> GetUserAdminMenu()
        {
            var list = await _menuService.GetUserAdminMenu();
            return Ok(list);
        }
    }
}
