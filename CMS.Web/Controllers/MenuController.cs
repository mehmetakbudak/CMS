﻿using CMS.Service;
using CMS.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [HttpGet("menu/frontend")]
        [ProducesResponseType(typeof(List<MenuModel>), 200)] //OK
        public async Task<IActionResult> GetFrontendMenu()
        {
            const string key = "frontEndMenu";

            if (_memoryCache.TryGetValue(key, out object menus))
            {
                return Ok(menus);
            }

            menus = await _menuService.GetFrontendTreeMenu();
            _memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });
            return Ok(menus);
        }
        #endregion
    }
}
