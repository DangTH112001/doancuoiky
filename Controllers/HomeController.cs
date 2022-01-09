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
            return View();
        }
      
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int id = context.getID(username, password);
            string name = context.getName(id);
            return Json(new {id = id, name = name});
        }

        [HttpPost]
        public JsonResult Register(string username, string password, string repassword, string fullname) {

            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (context.existUser(username) == 1) {
                return Json(new {
                    id = -1, 
                    name = ""
                });
            }

            if (password != repassword) {
                return Json(new {
                    id = -3, 
                    name = ""
                });
            }
            int errorCode = context.addUser(username, password, fullname);
            if (errorCode > 0) {
                string name = context.getName(errorCode);
                return Json(new {
                    id = errorCode,
                    name = name
                });
            }
            return Json(new {
                id = errorCode, 
                name = ""
            });
        }

        public IActionResult Home()
        {
            ViewData["css_path"] = "/css/Home.css";
            return View();
        }

        public IActionResult Bookmark()
        {
            ViewData["css_path"] = "/css/Bookmark.css";
            return View();
        }

        public IActionResult Question()
        {
            ViewData["css_path"] = "/css/Question.css";
            return View();
        }

        public IActionResult Quiz()
        {
            ViewData["css_path"] = "/css/Quiz.css";
            return View();
        }

    }
}
