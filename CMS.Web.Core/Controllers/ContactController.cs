using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Controllers
{    
    public class ContactController : Controller
    {
        [Route("iletisim")]
        public IActionResult Index()
        {
            return View();
        }
    }
}