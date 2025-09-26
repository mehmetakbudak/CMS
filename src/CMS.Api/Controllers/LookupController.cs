using CMS.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api.Controllers
{
    public class LookupController(ILookupService lookupService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> ContactCategories()
        {
            var result = await lookupService.ContactCategories();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TaskCategories()
        {
            var result = await lookupService.TaskCategories();
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> TaskStatuses(int categoryId)
        {
            var result = await lookupService.TaskStatuses(categoryId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var result = await lookupService.Users();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Menus()
        {
            var result = await lookupService.Menus();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> BlogCategories()
        {
            var result = await lookupService.BlogCategories();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult MethodTypes()
        {
            var result = lookupService.MethodTypes();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult UserTypes()
        {
            var result = lookupService.UserTypes();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult UserStatuses()
        {
            var result = lookupService.UserStatuses();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult WorkTypes()
        {
            var result = lookupService.WorkTypes();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Roles()
        {
            var result = await lookupService.Roles();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> JobLocations()
        {
            var result = await lookupService.JobLocations();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Tags()
        {
            var result = await lookupService.Tags();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Tenants()
        {
            var result = await lookupService.Tenants();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult CommentStatuses()
        {
            var result = lookupService.CommentStatuses();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult MenuTypes()
        {
            var result = lookupService.MenuTypes();
            return Ok(result);
        }
    }
}
