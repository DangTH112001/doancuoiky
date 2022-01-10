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
    public class BookmarkController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/Bookmark/Index.css";
            return View();
        }
    }
}
