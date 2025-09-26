using CMS.Business.Services;
using CMS.Storage.Dtos.Language;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class LanguageValueController(ILanguageValueService languageValueService) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Get([FromBody] LanguageValueFilterDto dto)
        {
            var result = await languageValueService.Get(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await languageValueService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LanguageValueItemDto dto)
        {
            var result = await languageValueService.Create(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LanguageValueItemDto dto)
        {
            var result = await languageValueService.Update(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await languageValueService.Delete(id);
            return Ok(result);
        }
    }
}
