using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using doancuoiky.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace doancuoiky.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            ViewData["css_path"] = "/css/Home/Index.css";
            return View();
        }

        [HttpPost]
        public JsonResult ScrollPage(int limit, int start)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Multiplechoice> list = new List<Multiplechoice>();
            list = context.FetchMultiplechoicesToHome(limit, start);
            return Json(list);
        }

        [HttpGet]
        public JsonResult FindQuiz(string filter, string text)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Multiplechoice> list = new List<Multiplechoice>();
            list = context.FindQuiz(filter, text);
            return Json(list);
        }

        public IActionResult StartQuiz(int id)
        {
            ViewData["id"] = id;
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<object> list = new List<object>();
            list = context.GetQuiz(id);
            return View(list);
        }

        [HttpPost]
        public JsonResult SubmitQuiz(int quizID, string answerList, int quesNum, int userID)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            var data = JsonConvert.DeserializeObject<List<AnswerList>>(answerList);

            Double totalScore = 0;

            foreach(var item in data)
            {
                totalScore += checkResultOfQuestion(quizID, item.id, item.opt);
            }

            totalScore = (totalScore / quesNum) * 10;

            return Json(totalScore);
        }

        public int checkResultOfQuestion(int QuizID, int QuestionID, string Opt)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            int Score = 0;
            List<AnswerList> list = context.checkResult(QuizID);

            for(int i = 0; i < list.Count; i++)
            {
                if(QuestionID.Equals(list[i].id))
                {
                    if(Opt.Equals(list[i].opt))
                    {
                        Score += 1;
                        break;
                    }
                }
            }
            return Score;
        }

        [HttpGet]
        public IActionResult ReviewQuiz(int id)
        {
            StoreContext context = HttpContext.RequestServices.GetService(typeof(doancuoiky.Models.StoreContext)) as StoreContext;
            List<Question> list = new List<Question>();
            list = context.GetResultByQuizID(id);
            ViewBag.Data = list;
            return View();
        }

        [HttpPost]
        public string TestCode(int quizID, string answerList, int quesNum, int userID)
        {
            string s = "";
            return s;
        }

    }
}
