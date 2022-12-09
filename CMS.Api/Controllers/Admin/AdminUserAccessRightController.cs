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
        public async Task<IActionResult> Get(int userId)
        {
            var list = await userAccessRightService.Get(userId);
            return Ok(list);
        }

        [HttpPut("CreateOrUpdate")]
        public async Task<IActionResult> CreateOrUpdate([FromBody]UserAccessRightModel model)
        {
            var result = await userAccessRightService.CreateOrUpdate(model);
            return Ok(result);
        }
    }
}
