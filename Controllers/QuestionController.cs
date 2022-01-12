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
                list = context.getQuestions(filter, AuthController.uid);
            return Json(list);
        }

        [HttpPost]
        public JsonResult getQuestions(int mcid) {
            List<Question> list = new List<Question>();
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            list = context.getQuestionsfromQuiz(mcid);
            return Json(list);
        }

        public IActionResult Insert(int uid) {
            ViewData["css_path"] = "/css/Question/Insert.css";
            ViewData["uid"] = uid;
            return View();
        }

        public IActionResult Preview(string list) {
            ViewData["css_path"] = "/css/Question/Preview.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            string[] ds = list.Split('_');
            return View(context.getQuestions(ds));
        }

        public IActionResult Them(string question, string filter, string A, string B, string C, string D, string answer, int uid) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.themQuestion(question, filter, A, B, C, D, answer, uid);
            if (err != -1) {
                return RedirectToAction("Index", "Question");
            }
            else {
                return View();
            }
        }

        public IActionResult CapNhat(int id, string question, string filter, string A, string B, string C, string D, string answer) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.capnhatQuestion(id, question, filter, A, B, C, D, answer);
            if (err != -1) {
                return RedirectToAction("Index", "Question");
            }
            else {
                return View();
            }
        }

        public IActionResult Xoa(int qid) {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int err = context.xoaQuestion(qid);
            if (err != -1) {
                return RedirectToAction("Index", "Question");
            }
            else {
                return View();
            }
        }

        public IActionResult Update(int qid) {
            ViewData["css_path"] = "/css/Question/Update.css";
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            return View(context.getQuestion(qid));
        }
    }
}