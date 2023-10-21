using CMS.Service;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        [HttpGet("team/list")]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = await _teamService.GetAll();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }
        #endregion
    }
}
