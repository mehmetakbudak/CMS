using CMS.Model.Model;
using CMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserAccessRightController : ControllerBase
    {
        private readonly IUserAccessRightService userAccessRightService;

        public AdminUserAccessRightController(IUserAccessRightService userAccessRightService)
        {
            this.userAccessRightService = userAccessRightService;
        }


        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var list = userAccessRightService.Get(userId);
            return Ok(list);
        }

        [HttpPut("CreateOrUpdate")]
        public IActionResult CreateOrUpdate([FromBody]UserAccessRightModel model)
        {
            var result = userAccessRightService.CreateOrUpdate(model);
            return StatusCode(result.StatusCode, result);
        }
    }
}
