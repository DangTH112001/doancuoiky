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

        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return Json(context.getID(username, password));
        }

        [HttpPost]
        public JsonResult Register(string username, string password, string repassword, string fullname) {

            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (context.existUser(username) == 1) return Json(-1);
            if (password != repassword) return Json(-3);
            int errorCode = context.addUser(username, password, fullname);
            return Json(errorCode);
        }
    }
}
