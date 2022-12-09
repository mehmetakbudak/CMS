using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Api
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminAccessRightController : ControllerBase
    {
        private readonly IAccessRightService _accessRightService;
        public AdminAccessRightController(IAccessRightService accessRightService)
        {
            _accessRightService = accessRightService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _accessRightService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _accessRightService.Get(id);
            return Ok(result);
        }

        [HttpPost("Menu")]
        public async Task<IActionResult> PostMenu([FromBody] AccessRightModel model)
        {
            var result = await _accessRightService.PostMenu(model);
            return Ok(result);
        }

        [HttpPut("Menu")]
        public async Task<IActionResult> PutMenu([FromBody] AccessRightModel model)
        {
            var result = await _accessRightService.PutMenu(model);
            return Ok(result);
        }

        [HttpPost("Operation")]
        public async Task<IActionResult> PostOperation([FromBody] AccessRightModel model)
        {
            var result = await _accessRightService.PostOperation(model);
            return Ok(result);
        }

        [HttpPut("Operation")]
        public async Task<IActionResult> PutOperation([FromBody] AccessRightModel model)
        {
            var result = await _accessRightService.PutOperation(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accessRightService.Delete(id);
            return Ok(result);
        }
    }
}