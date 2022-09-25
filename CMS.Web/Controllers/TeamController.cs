using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class TeamController : Controller
    {
        #region Constructor
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region APIs
        [HttpGet("api/team")]
        public IActionResult Get()
        {
            var list = _teamService.GetAll();
            return Ok(list);
        }
        #endregion
    }
}
