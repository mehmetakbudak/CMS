using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CMS.Api.Controllers.Admin
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminMenuController : ControllerBase
    {
        private readonly IAccessRightService accessRightService;
        private IMemoryCache memoryCache;

        public AdminMenuController(IAccessRightService accessRightService,
            IMemoryCache memoryCache)
        {
            this.accessRightService = accessRightService;
            this.memoryCache = memoryCache;
        }

        [HttpGet("GetUserMenu")]
        public IActionResult GetUserMenu()
        {
            string key = $"userMenu_{AuthTokenContent.Current.UserId}";

            if (memoryCache.TryGetValue(key, out object menus))
                return Ok(menus);

            menus = accessRightService.GetUserMenu();
            memoryCache.Set(key, menus, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.Normal
            });

            return Ok(menus);
        }

    }
}
