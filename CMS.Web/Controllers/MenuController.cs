using CMS.Storage.Dto;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Web.Controllers
{
    public class MenuController : Controller
    {
        #region Constructor
        private readonly IMenuService _menuService;
        private IMemoryCache _memoryCache;

        public MenuController(IMenuService menuService,
            IMemoryCache memoryCache)
        {
            _menuService = menuService;
            _memoryCache = memoryCache;
        }
        #endregion

        #region APIs
        [HttpGet("api/menu/frontend")]
        [ProducesResponseType(typeof(List<MenuModel>), 200)] //OK
        public IActionResult GetFrontendMenu()
        {
            const string key = "frontEndMenu";

            if (_memoryCache.TryGetValue(key, out object menus))
            {
                return Ok(menus);
            }

            menus = _menuService.GetFrontendMenu();
            _memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });
            return Ok(menus);
        }
        #endregion
    }
}
