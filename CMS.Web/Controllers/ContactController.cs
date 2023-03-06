using CMS.Storage.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Controllers
{
    public class ContactController : Controller
    {
        #region Constructor
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        #endregion

        #region Views
        public IActionResult Index() => View();

        #endregion

        #region APIs
        [HttpPost("api/contact")]
        public async Task<IActionResult> Post([FromBody] ContactModel model)
        {
            var result = await _contactService.Post(model);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
