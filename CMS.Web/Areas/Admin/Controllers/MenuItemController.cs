using CMS.Service;
using CMS.Service.Attributes;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }            

        [HttpGet("admin/menu-item/{id}")]
        [CMSAuthorize(RouteLevel = 2)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _menuItemService.GetById(id);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPost("admin/menu-item")]
        public async Task<IActionResult> Post([FromBody] MenuItemModel model)
        {
            var result = await _menuItemService.Post(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpPut("admin/menu-item")]
        public async Task<IActionResult> Put([FromBody] MenuItemModel model)
        {
            var result = await _menuItemService.Put(model);
            return Ok(result);
        }

        [CMSAuthorize(RouteLevel = 2)]
        [HttpDelete("admin/menu-item/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _menuItemService.Delete(id);
            return Ok(result);
        }
    }
}
