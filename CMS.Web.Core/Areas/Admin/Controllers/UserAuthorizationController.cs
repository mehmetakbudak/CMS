﻿using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Areas.Admin.Controllers
{
    //[CMSAuthorize]
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class UserAuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}