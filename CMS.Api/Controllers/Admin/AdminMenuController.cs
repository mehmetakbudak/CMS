using CMS.Service;
using CMS.Model.Model;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Api.Controllers.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminMenuController : ControllerBase
    {
        private readonly IAccessRightService _accessRightService;
        private readonly IMenuService _menuService;
        private readonly IMemoryCache _memoryCache;

        public AdminMenuController(IAccessRightService accessRightService,
            IMemoryCache memoryCache,
            IMenuService menuService)
        {
            _accessRightService = accessRightService;
            _memoryCache = memoryCache;
            _menuService = menuService;
        }

        [HttpGet("FrontendMenu")]
        public IActionResult GetFrontendMenu()
        {
            var list = _menuService.GetFrontendTreeMenu();
            return Ok(list);
        }

        [HttpPost("FrontendMenu")]
        public IActionResult PostFrontendMenu([FromBody]TreeDataModel model)
        {
            var result = _menuService.PostFrontendMenu(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("FrontendMenu")]
        public IActionResult PutFrontendMenu([FromBody] TreeDataModel model)
        {
            var result = _menuService.PutFrontendMenu(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("FrontendMenu/{id}")]
        public IActionResult DeleteFrontendMenu(int id)
        {
            var result = _menuService.DeleteFrontendMenu(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetUserMenu")]
        public IActionResult GetUserMenu()
        {
            string key = $"userMenu_{AuthTokenContent.Current.UserId}";

            if (_memoryCache.TryGetValue(key, out object menus))
            {
                return Ok(menus);
            }

            menus = _accessRightService.GetUserMenu();

            _memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });

            return Ok(menus);
        }

        [HttpGet("GetPageMenus")]
        public IActionResult GetPageMenus()
        {
            return Ok();
        }
    }
}
