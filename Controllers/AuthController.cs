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
    public class AuthController : Controller
    {
        public static int uid;
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
            if (id > 0) 
                uid = id;
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
                uid = errorCode;
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
    }
}
