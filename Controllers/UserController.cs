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
    public class UserController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/User/Index.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Object> list = new List<object>();

            var obj = new
                        {
                            name = context.getName(AuthController.uid),
                            SLTG = context.getSLTG(AuthController.uid),
                            SLT = context.getSLT(AuthController.uid),
                            rank = context.getRank(AuthController.uid)
                        };
            list.Add(obj);
            return View(list);
        }
    }
}