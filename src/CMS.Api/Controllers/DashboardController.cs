using CMS.Business.Attributes;
using CMS.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    [CMSAuthorize]
    public class DashboardController(IDashboardService dashboardService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var result = await dashboardService.GetCount();
            return Ok(result);
        }
    }
}
