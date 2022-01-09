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

        public int Login(string username, string password)
        {

            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            if (context.existUser(username, password)) {
                return 1;
            }
            else {
                return 0;
            }
        }

        public int Register(string username, string password, string repassword, string fullname) {

            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int errorCode = context.addUser(username, password, fullname);
            Console.WriteLine(username + " " + password + " " + fullname);

            return 1;
        }
    }
}
