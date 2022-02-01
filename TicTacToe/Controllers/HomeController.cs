using System.Web.Mvc;

namespace TicTacToe.Controllers
{
    
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}