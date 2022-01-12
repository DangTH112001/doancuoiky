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
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return View(context.getBookmark(AuthController.uid));
        }

        public IActionResult Xoa(int mcid) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.xoaBookmark(AuthController.uid, mcid);
            if (err != -1) {
                return RedirectToAction("Index", "Bookmark");
            }
            else {
                return View();
            }
        }
    }
}
