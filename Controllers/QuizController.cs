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
    public class QuizController : Controller
    {
        public IActionResult Index() {
            ViewData["css_path"] = "/css/Quiz/Index.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return View(context.getMultiplechoices(AuthController.uid));
        }

        public IActionResult ThemQuiz(string title, string description, int time, int[] qids) {
            ViewData["css_path"] = "/css/Quiz/ThemQuiz.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int mcid = context.addQuiz(title, description, time, AuthController.uid);
            if (mcid > 0) {
                int err = context.addBelong(qids, mcid);
                if (err != -1) {
                    return RedirectToAction("Index", "Home");
                }
                else {
                    return View();
                }
            }
            return View();
        }

        public IActionResult Delete(int mcid) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.xoaQuiz(mcid);
            if (err != -1) {
                return RedirectToAction("Index", "Quiz");
            }
            else {
                return View();
            }
        }

        public IActionResult Update(int mcid) {
            ViewData["css_path"] = "/css/Quiz/Update.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return View(context.getMultiplechoice(mcid));
        }

        public IActionResult Sua(int id, string title, string description, int time) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.capnhatQuiz(id, title, description, time);
            if (err != -1) {
                return RedirectToAction("Index", "Quiz");
            }
            else {
                return View();
            }
        }
    }
}
