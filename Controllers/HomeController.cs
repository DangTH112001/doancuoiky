using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using doancuoiky.Models;

namespace doancuoiky.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/Home/Index.css";
            return View();
        }
    }
}
