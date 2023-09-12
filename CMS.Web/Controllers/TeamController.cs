using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public IActionResult Index() => View();

        #endregion

        #region APIs
        [HttpGet("api/team")]
        public async Task<IActionResult> Get()
        {
            var list = await _teamService.GetAll();
            return Ok(list);
        }
        #endregion
    }
}
