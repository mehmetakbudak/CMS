using CMS.Business.Attributes;
using CMS.Business.Helper;
using CMS.Business.Services;
using CMS.Storage.Dtos.Language;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class LanguageController(
        ILanguageService languageService,
        ILanguageHelper languageHelper) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var response = languageService.Get();
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetDictionary()
        {
            var response = languageHelper.GetDictionary();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public  IActionResult GetById(int id)
        {
            var response = languageService.GetById(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanguageDto dto)
        {
            var response = await languageService.Create(dto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageDto dto)
        {
            var response = await languageService.Update(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await languageService.Delete(id);
            return Ok(response);
        }
    }
}
