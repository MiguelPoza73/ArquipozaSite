using Microsoft.AspNetCore.Mvc;

namespace Arquipoza.Module.Controllers
{
    public class SketchbookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}