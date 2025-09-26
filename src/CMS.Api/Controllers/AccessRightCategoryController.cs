using CMS.Business.Attributes;
using CMS.Business.Services;
using CMS.Storage.Dtos.AcessRightCategory;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class AccessRightCategoryController(
        IAccessRightCategoryService accessRightCategoryService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = accessRightCategoryService.Get();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWithAccessRights()
        {
            var result = await accessRightCategoryService.GetWithAccessRights();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await accessRightCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccessRightCategoryDto dto)
        {
            var result = await accessRightCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AccessRightCategoryDto dto)
        {
            var result = await accessRightCategoryService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await accessRightCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
