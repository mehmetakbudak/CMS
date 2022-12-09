using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    //[CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public AdminLookupController(
            ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet("ContactCategories")]
        public IActionResult GetContactCategories()
        {
            var list = _lookupService.ContactCategories();
            return Ok(list);
        }       

        [HttpGet("TodoCategories")]
        public IActionResult GetTodoCategories()
        {
            var list = _lookupService.TodoCategories();
            return Ok(list);
        }

        [HttpGet("TodoStatuses/{categoryId}")]
        public IActionResult GetTodoStatuses(int categoryId)
        {
            var list = _lookupService.TodoStatuses(categoryId);
            return Ok(list);
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var list = _lookupService.Users();
            return Ok(list);
        }

        [HttpGet("Menus")]
        public IActionResult GetMenus()
        {
            var list = _lookupService.Menus();
            return Ok(list);
        }

        [HttpGet("BlogCategories")]
        public IActionResult GetBlogCategories()
        {
            var list = _lookupService.BlogCategories();
            return Ok(list);
        }

        [HttpGet("MethodTypes")]
        public IActionResult GetMethodTypes()
        {
            var list = _lookupService.MethodTypes();
            return Ok(list);
        }

        [HttpGet("UserTypes")]
        public IActionResult UserTypes()
        {
            var list = _lookupService.UserTypes();
            return Ok(list);
        }
    }
}