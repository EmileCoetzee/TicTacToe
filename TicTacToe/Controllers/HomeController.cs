using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToe.DataAccess;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class HomeController : Controller
    {
        Query _query;
        Ganss.XSS.HtmlSanitizer _htmlSanitizer;

        public HomeController()
        {
            _query = new Query();
            _htmlSanitizer = new Ganss.XSS.HtmlSanitizer();
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/Start 
        public ActionResult Start()
        {
            return View();
        }

        // GET: Home/GetPlayer1Form (Partial)
        public PartialViewResult GetPlayer1Form()
        {
            Player player = new Player();

            player.isPlayer1 = true;

            return PartialView(player);
        }

        // GET: Home/GetPlayer2Form (Partial)
        public PartialViewResult GetPlayer2Form()
        {
            Player player = new Player();

            player.isPlayer1 = false;

            return PartialView(player);
        }

        // POST: Home/AddPlayer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPlayer(Player model)
        {

            string redirectURL = "";

            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Error, some data is missing.  Please ensure that all fields are entered.");

                

                model.Name = _htmlSanitizer.Sanitize(model.Name);

                //check if player exists
                int playerId = _query.CheckIfPlayerExists(model.Name);

                if (!model.isPlayer1)
                    redirectURL = "/Home/Play";
                
                    

                return Json(new { data = redirectURL, resultCode = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Home/Play 
        public ActionResult Play()
        {
            return View();
        }

        // GET: Home/GetBoard (Partial)
        public PartialViewResult GetBoard(int id)
        {
            ViewBag.CurrentRound = id;

            return PartialView();

        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}