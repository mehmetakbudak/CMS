using CMS.Business.Attributes;
using CMS.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class TaskStatusController(ITaskStatusService taskStatusService) : BaseController
    {
        [HttpGet]
        [EnableQueryWithMetadata]
        public IActionResult Get()
        {
            var result = taskStatusService.Get();
            return Ok(result);
        }
    }
}
