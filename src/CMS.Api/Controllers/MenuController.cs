using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.Menu;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class MenuController(IMenuService menuService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = menuService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await menuService.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdminMenu()
        {
            var response = await menuService.GetUserAdminMenu();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFrontendMenu()
        {
            var response = await menuService.GetFrontendMenu();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetFrontendTreeMenu()
        {
            var response = await menuService.GetFrontendTreeMenu();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuDto menuDto)
        {
            var response = await menuService.Create(menuDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MenuDto menuDto)
        {
            var response = await menuService.Update(menuDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await menuService.Delete(id);
            return Ok(response);
        }
    }
}
