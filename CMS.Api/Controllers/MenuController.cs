using CMS.Storage.Dto;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IAccessRightService accessRightService;
        private readonly IMenuService menuService;
        private readonly IMemoryCache memoryCache;

        public MenuController(IAccessRightService accessRightService,
            IMenuService menuService,
            IMemoryCache memoryCache)
        {
            this.accessRightService = accessRightService;
            this.menuService = menuService;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// Ön arayüz menü elemanlarını döner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Frontend")]
        [ProducesResponseType(typeof(List<MenuModel>), 200)] //OK
        public async Task<IActionResult> GetFrontendMenu()
        {
            const string key = "frontEndMenu";

            if (memoryCache.TryGetValue(key, out object menus))
            {
                return Ok(menus);
            }

            menus = await menuService.GetFrontendMenu();
            memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });
            return Ok(menus);
        }
    }
}