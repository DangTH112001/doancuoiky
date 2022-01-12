using Microsoft.AspNetCore.Mvc;

namespace doancuoiky.Controllers
{
    public class QuizzingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}