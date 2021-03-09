using CMS.Model.Dto;
using CMS.Model.Helper;
using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService userGroupService;
        public RoleController(IRoleService userGroupService)
        {
            this.userGroupService = userGroupService;
        }

        [HttpGet]
        public IActionResult Get(int page)
        {
            var list = userGroupService.GetAll(page);
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var model = userGroupService.GetById(id);
            return Ok(model);
        }

        [HttpPost("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody] RoleModel model)
        {
            var result = userGroupService.CreateOrUpdate(model);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = userGroupService.Delete(id);
            return StatusCode(result.IntStatusCode, HttpHelper.Result(result));
        }
    }
}