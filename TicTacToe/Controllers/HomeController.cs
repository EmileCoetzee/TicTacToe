using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToe.DataAccess;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    [System.Runtime.InteropServices.Guid("CE5AC464-DD4C-45BE-80E2-CD24A9D0166A")]
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


        // POST: Home/AddPlayer
        [HttpPost]
        [Route("Home/SaveMove/{gameId}/{currentRound}/{currentPlayer}/{blockId}")]
        public JsonResult SaveMove(int gameId, int currentRound, int currentPlayer, int blockId)
        {
            try
            {

                //add move (moves table)

                Move move = _query.UpdateMoves(gameId, currentPlayer, blockId);

                if (move is null)
                    throw new Exception("Error, could not save move, please try again.");

                int[] winningSequence1 = { 0, 4, 8 };

                //int[] winningSequence1 = { 1, 0, 0, 0, 1, 0, 0, 0, 1 };


                int[] moves = { move.B1, move.B2, move.B3, move.B4, move.B5, move.B6, move.B7, move.B8, move.B9 };

                //bool isEqual = winningSequence1.SequenceEqual(moves);


                //List<>.IndexOf(moves, 1);

                List<int> checkingSequence = new List<int>();

                int[] players = { 1, 2 };



                foreach (var p in players)
                {
                    for (int i = 0; i < moves.Length; i++)
                    {
                        if (moves[i] == p)
                        {
                            checkingSequence.Add(i);
                        }
                    }

                    bool isEqual = winningSequence1.SequenceEqual(checkingSequence);

                    if (isEqual)
                        break;
                }


                    //    List<int> playerMoves = new List<int>();

                    //    foreach (var m in moves)
                    //    {
                    //        //if (m == p)
                    //        //{
                    //            playerMoves.Add(m);
                    //        //}

                    //        bool isEqual = winningSequence1.SequenceEqual(playerMoves);
                    //    }
                    //    //count occurrances
                    //    //if (moveFields.Count(x => x == p) > 2)
                    //   // {
                    //        //bool isEqual = winningSequence1.SequenceEqual();
                    //    //}

                    //}



                    //if winner then clear moves table

                    //then add points and set highest round reached (game table)


                    return Json(new { data = "", resultCode = 1, player1Id = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}