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
    public class BelongController : Controller
    {
        public IActionResult Xoa(int qid, int mcid) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.xoaBelong(qid, mcid);
            if (err != -1) {
                return RedirectToAction("Update", "Quiz", new {mcid = mcid});
            }
            else {
                return View();
            }
        }
    }
}
