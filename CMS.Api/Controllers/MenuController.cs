using CMS.Model.Dto;
using CMS.Model.Entity;
using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace CMS.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IAccessRightService accessRightService;
        private readonly IMenuService menuService;
        private IMemoryCache memoryCache;

        public MenuController(IAccessRightService accessRightService,
            IMenuService menuService,
            IMemoryCache memoryCache)
        {
            this.accessRightService = accessRightService;
            this.menuService = menuService;
            this.memoryCache = memoryCache;
        }

        [HttpGet("Frontend")]
        public IActionResult GetFrontendMenu()
        {
            const string key = "frontEndMenu";

            if (memoryCache.TryGetValue(key, out object menus))
                return Ok(menus);

            menus = menuService.GetFrontendMenu();
            memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });
            return Ok(menus);
        }        
    }
}