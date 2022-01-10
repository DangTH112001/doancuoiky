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
    public class QuizController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/Quiz/Index.css";
            return View();
        }
    }
}
