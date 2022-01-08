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

        public IActionResult Login(string username, string password)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (context.existUser(username, password)) {
                return View("Home", "Home");
            }
            else {
                return View("Index", "Home");
            }
        }

        public IActionResult Register(string username, string password, string repassword, string fullname) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (password == repassword && context.addUser(username, password, fullname) != -1) {
                return View("Home", "Home");
            }
            return View("Index", "Home");
        }
    }
}
