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
        private readonly IContactCategoryService _contactCategoryService;
        private readonly ITodoCategoryService _todoCategoryService;
        private readonly ITodoStatusService _todoStatusService;
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly IBlogCategoryService _blogCategoryService;

        public AdminLookupController(
            IContactCategoryService contactCategoryService,
            ITodoCategoryService todoCategoryService,
            ITodoStatusService todoStatusService,
            IBlogCategoryService blogCategoryService,
            IUserService userService,
            IMenuService menuService)
        {
            _contactCategoryService = contactCategoryService;
            _todoCategoryService = todoCategoryService;
            _todoStatusService = todoStatusService;
            _userService = userService;
            _menuService = menuService;
            _blogCategoryService = blogCategoryService; 
        }

        [HttpGet("ContactCategories")]
        public IActionResult GetContactCategories()
        {
            var list = _contactCategoryService.Lookup();
            return Ok(list);
        }       

        [HttpGet("TodoCategories")]
        public IActionResult GetTodoCategories()
        {
            var list = _todoCategoryService.Lookup();
            return Ok(list);
        }

        [HttpGet("TodoStatuses/{categoryId}")]
        public IActionResult GetTodoStatuses(int categoryId)
        {
            var list = _todoStatusService.Lookup(categoryId);
            return Ok(list);
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var list = _userService.Lookup();
            return Ok(list);
        }

        [HttpGet("Menus")]
        public IActionResult GetMenus()
        {
            var list = _menuService.Lookup();
            return Ok(list);
        }

        [HttpGet("BlogCategories")]
        public IActionResult GetBlogCategories()
        {
            var list = _blogCategoryService.Lookup();
            return Ok(list);
        }
    }
}