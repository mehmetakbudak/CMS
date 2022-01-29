using CMS.Service;
using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Api
{
    [CMSAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminLookupController : ControllerBase
    {
        private readonly IContactCategoryService contactCategoryService;
        private readonly ITodoCategoryService todoCategoryService;
        private readonly ITodoStatusService todoStatusService;
        private readonly IUserService userService;
        private readonly IMenuService menuService;

        public AdminLookupController(
            IContactCategoryService contactCategoryService,
            ITodoCategoryService todoCategoryService,
            ITodoStatusService todoStatusService,
            IUserService userService,
            IMenuService menuService)
        {
            this.contactCategoryService = contactCategoryService;
            this.todoCategoryService = todoCategoryService;
            this.todoStatusService = todoStatusService;
            this.userService = userService;
            this.menuService = menuService;
        }

        [HttpGet("ContactCategories")]
        public IActionResult GetContactCategories()
        {
            var list = contactCategoryService.Lookup();
            return Ok(list);
        }       

        [HttpGet("TodoCategories")]
        public IActionResult GetTodoCategories()
        {
            var list = todoCategoryService.Lookup();
            return Ok(list);
        }

        [HttpGet("TodoStatuses/{categoryId}")]
        public IActionResult GetTodoStatuses(int categoryId)
        {
            var list = todoStatusService.Lookup(categoryId);
            return Ok(list);
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var list = userService.Lookup();
            return Ok(list);
        }

        [HttpGet("Menus")]
        public IActionResult GetMenus()
        {
            var list = menuService.Lookup();
            return Ok(list);
        }
    }
}