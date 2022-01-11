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
        public IActionResult Index() 
        {
            ViewData["css_path"] = "/css/Home/Index.css";
            return View();
        }

        [HttpPost]
        public JsonResult ScrollPage(int limit, int start)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Multiplechoice> list = new List<Multiplechoice>();
            list = context.FetchMultiplechoicesToHome(limit, start);
            return Json(list);
        }

        [HttpGet]
        public JsonResult FindQuiz(string filter, string text)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Multiplechoice> list = new List<Multiplechoice>();
            list = context.FindQuiz(filter, text);
            return Json(list);
        }

        [HttpGet]
        public string TestCode()
        {
            string s = "";
            return s;
        }

    }
}
