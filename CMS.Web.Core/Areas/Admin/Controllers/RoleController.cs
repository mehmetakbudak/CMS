﻿using CMS.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Web.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    public class RoleController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }
    }
}