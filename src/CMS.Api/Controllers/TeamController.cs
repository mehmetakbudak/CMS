using CMS.Business.Services;
using CMS.Storage.Dtos.Filter;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class TeamController(ITeamService teamService) : BaseController
    {
        [HttpPost]
        public IActionResult GetAllActive(PagerDto dto)
        {
            var result = teamService.GetAllActive(dto);
            return Ok(result);
        }

    }
}
