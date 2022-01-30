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

            PlayerStartViewModel vm = new PlayerStartViewModel();
            vm.Player = player;

            return PartialView(vm);
        }

        // GET: Home/GetPlayer2Form (Partial)
        public PartialViewResult GetPlayer2Form()
        {
            Player player = new Player();

            player.isPlayer1 = false;

            PlayerStartViewModel vm = new PlayerStartViewModel();
            vm.Player = player;

            return PartialView(vm);
        }

        // POST: Home/AddPlayer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddPlayer(PlayerStartViewModel model)
        {

            string redirectURL = "";

            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Error, some data is missing.  Please ensure that all fields are entered.");

                

                model.Player.Name = _htmlSanitizer.Sanitize(model.Player.Name);

                //check if player exists
                int playerId = _query.CheckIfPlayerExists(model.Player.Name);

                if (!model.Player.isPlayer1)
                    redirectURL = "/Home/Play/" + model.Player1Id + "/" + playerId;
                    

                return Json(new { data = redirectURL, resultCode = 1, player1Id = playerId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Home/Play/1/1
        [Route("Home/Play/{player1Id}/{player2Id}")]
        public ActionResult Play(int player1Id, int player2Id)
        {
            //create new game
            int gameId = _query.CreateGame(player1Id, player2Id);

            ViewBag.GameId = gameId;


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