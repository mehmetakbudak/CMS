using CMS.Model.Dto;
using CMS.Model.Enum;
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
        private IMemoryCache memoryCache;

        public MenuController(IAccessRightService accessRightService, IMemoryCache memoryCache)
        {
            this.accessRightService = accessRightService;
            this.memoryCache = memoryCache;
        }

        [Route("frontend")]
        public IActionResult GetFrontendMenu()
        {
            const string key = "frontEndMenu";

            if (memoryCache.TryGetValue(key, out object menus))
                return Ok(menus);

            menus = accessRightService.GetFrontEndMenu();
            memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });
            return Ok(menus);
        }

        [CMSApiAuthorize]
        [Route("backend")]
        public IActionResult GetBackEndMenu()
        {
            List<AccessRightModel> list = new List<AccessRightModel>();
            var userId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetInt32("UserType");
            if (userId.HasValue && userType.HasValue)
            {
                list = accessRightService.GetBackEndMenuByUserId(userId.Value, userType.Value);
            }
            return Ok(list);
        }
    }
}