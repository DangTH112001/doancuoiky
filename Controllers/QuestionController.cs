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
    public class QuestionController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/Question/Index.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return View(context.getQuestions(AuthController.uid));
        }

        public JsonResult Filter(string filter) {
            List<Question> list = new List<Question>();
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (filter == "all") 
                list = context.getQuestions(AuthController.uid);
            else 
                list = context.getQuestions(filter);
            return Json(list);
        }

        public IActionResult Update(int qid) {
            ViewData["css_path"] = "/css/Question/Update.css";
            return View();
        }

        public IActionResult Insert() {
            ViewData["css_path"] = "/css/Question/Insert.css";
            return View();
        }
    }
}