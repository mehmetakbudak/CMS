using CMS.Service;
using CMS.Storage.Model;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetFrontendMenu()
        {
            var list = await _menuService.GetFrontendTreeMenu();
            return Ok(list);
        }

        [HttpPost("FrontendMenu")]
        public async Task<IActionResult> PostFrontendMenu([FromBody]TreeDataModel model)
        {
            var result = await _menuService.PostFrontendMenu(model);
            return Ok(result);
        }

        [HttpPut("FrontendMenu")]
        public async Task<IActionResult> PutFrontendMenu([FromBody] TreeDataModel model)
        {
            var result = await _menuService.PutFrontendMenu(model);
            return Ok(result);
        }

        [HttpDelete("FrontendMenu/{id}")]
        public async Task<IActionResult> DeleteFrontendMenu(int id)
        {
            var result = await _menuService.DeleteFrontendMenu(id);
            return Ok(result);
        }

        [HttpGet("GetUserMenu")]
        public async Task<IActionResult> GetUserMenu()
        {
            //string key = $"userMenu_{AuthTokenContent.Current.UserId}";

            //if (_memoryCache.TryGetValue(key, out object menus))
            //{
            //    return Ok(menus);
            //}

           var menus = await _accessRightService.GetUserMenu();

            //_memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            //{
            //    Priority = CacheItemPriority.Normal
            //});

            return Ok(menus);
        }

        [HttpGet("GetPageMenus")]
        public IActionResult GetPageMenus()
        {
            return Ok();
        }
    }
}
