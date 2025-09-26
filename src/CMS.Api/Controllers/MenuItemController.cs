using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Menu;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class MenuItemController(IMenuItemService menuItemService) : BaseController
    {
        [CMSAuthorize]
        [HttpGet("{menuId}")]
        public async Task<ActionResult> GetMenuItemsByMenuId(int menuId)
        {
            var result = await menuItemService.GetMenuItemsByMenuId(menuId);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await menuItemService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [CMSAuthorize]
        public async Task<ActionResult> Create([FromBody] MenuItemDto menuItemDto)
        {
            var result = await menuItemService.Create(menuItemDto);
            return Ok(result);
        }

        [HttpPut]
        [CMSAuthorize]
        public async Task<ActionResult> Update([FromBody] MenuItemDto menuItemDto)
        {
            var result = await menuItemService.Update(menuItemDto);
            return Ok(result);
        }

        [CMSAuthorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await menuItemService.Delete(id);
            return Ok(result);
        }
    }
}
