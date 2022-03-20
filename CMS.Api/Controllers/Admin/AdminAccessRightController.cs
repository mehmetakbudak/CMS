using CMS.Model.Enum;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult Get()
        {
            var result = _accessRightService.Get();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _accessRightService.Get(id);
            return Ok(result);
        }

        [HttpPost("Menu")]
        public IActionResult PostMenu([FromBody] AccessRightModel model)
        {
            var result = _accessRightService.PostMenu(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Menu")]
        public IActionResult PutMenu([FromBody] AccessRightModel model)
        {
            var result = _accessRightService.PutMenu(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Operation")]
        public IActionResult PostOperation([FromBody] AccessRightModel model)
        {
            var result = _accessRightService.PostOperation(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Operation")]
        public IActionResult PutOperation([FromBody] AccessRightModel model)
        {
            var result = _accessRightService.PutOperation(model);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _accessRightService.Delete(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}