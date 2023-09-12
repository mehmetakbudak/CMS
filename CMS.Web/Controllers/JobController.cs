using CMS.Service;
using CMS.Storage.Model;
using CMS.Web.Models;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMS.Web.Controllers
{
    [Route("[controller]")]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IUserJobService _userJobService;

        public JobController(
            IJobService jobService,
            IUserJobService userJobService)
        {
            _jobService = jobService;
            _userJobService = userJobService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _jobService.GetById(id);
            return View(result);
        }

        [HttpGet("active-jobs")]
        public async Task<IActionResult> GetActiveJobs(DataSourceLoadOptions loadOptions, string position, List<int> location, List<int> workType)
        {
            var list = await _jobService.GetActiveJobs(position, location, workType);
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpGet("applied-jobs")]
        public async Task<IActionResult> GetAppliedJobs(DataSourceLoadOptions loadOptions)
        {
            var list = await _userJobService.GetAppliedJobs();
            return Json(DataSourceLoader.Load(list, loadOptions));
        }

        [HttpPost("user-job")]
        public async Task<IActionResult> PostUserJob([FromBody] UserJobPostModel model)
        {
            var result = await _userJobService.Post(model);
            return Ok(result);
        }

        [HttpDelete("user-job/{id}")]
        public async Task<IActionResult> DeleteUserJob(int id)
        {
            var result = await _userJobService.Delete(id);
            return Ok(result);
        }

    }
}
