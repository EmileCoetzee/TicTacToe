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


            Player player1 = _query.GetPlayer(player1Id);
            player1.isPlayer1 = true;

            Player player2 = _query.GetPlayer(player2Id);

            PlayerPlayViewModel playerVM = new PlayerPlayViewModel();

            playerVM.Player1 = player1;
            playerVM.Player2 = player2;

            return View(playerVM);
        }

        // GET: Home/GetBoard/1/name (Partial)
        [Route("Home/GetBoard/{currentRound}/{currentPlayerName}")]
        public PartialViewResult GetBoard(int currentRound, string currentPlayerName)
        {
            currentPlayerName = _htmlSanitizer.Sanitize(currentPlayerName);

            BoardViewModel vm = new BoardViewModel();

            vm.Round = currentRound;
            vm.PlayerName = currentPlayerName;

            return PartialView(vm);

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

                //update player score (1 move = 1 point)
                _query.UpdateScore(gameId, currentPlayer, 1);

                if (move is null)
                    throw new Exception("Error, could not save move, please try again.");

                


                int[] moves = { move.B1, move.B2, move.B3, move.B4, move.B5, move.B6, move.B7, move.B8, move.B9 };

                int winner = 0;
                //List<int> checkingSequence = new List<int>();

                //check for win sequences

                //row 1 { 0, 1, 2 }
                if (moves[0] == currentPlayer && moves[1] == currentPlayer && moves[2] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //row 2 { 3, 4, 5 }
                if (moves[3] == currentPlayer && moves[4] == currentPlayer && moves[5] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //row 3 { 6, 7, 8 }
                if (moves[6] == currentPlayer && moves[7] == currentPlayer && moves[8] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //column 1 { 0, 3, 6 }
                if (moves[0] == currentPlayer && moves[3] == currentPlayer && moves[6] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //column 2 { 1, 4, 7 }
                if (moves[1] == currentPlayer && moves[4] == currentPlayer && moves[7] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //column 3 { 2, 5, 8 }
                if (moves[2] == currentPlayer && moves[5] == currentPlayer && moves[8] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //diagonal 1 { 0, 4, 8 }
                if (moves[0] == currentPlayer && moves[4] == currentPlayer && moves[8] == currentPlayer)
                {
                    winner = currentPlayer;
                }

                //diagonal 2 { 2, 4, 6 }
                if (moves[2] == currentPlayer && moves[4] == currentPlayer && moves[6] == currentPlayer)
                {
                    winner = currentPlayer;
                }


                if (winner != 0)
                {


                    //update player score (round winner gets 10 points)
                    _query.UpdateScore(gameId, currentPlayer, 10);

                    //update highest round completed
                    _query.UpdateHighestRoundCompleted(gameId, currentRound);

                    //increment round
                    currentRound++;


                    //clear moves table
                    if (_query.ClearMoves(gameId) != 1)
                        throw new Exception("Error, could not start next round, please try again.");


                    return Json(new { data = "", resultCode = 1, newRound = currentRound, winner = winner, draw = "" }, JsonRequestBehavior.AllowGet);
                }

                //ran out of moves
                if (!moves.Contains(0))
                {
                   
                    //update highest round completed
                    _query.UpdateHighestRoundCompleted(gameId, currentRound);

                    //increment round
                    currentRound++;


                    //clear moves table
                    if (_query.ClearMoves(gameId) != 1)
                        throw new Exception("Error, could not start next round, please try again.");

                    return Json(new { data = "It's a DRAW!", resultCode = 1, newRound = currentRound, winner = "", draw = 1 }, JsonRequestBehavior.AllowGet);
                }



                return Json(new { data = "", resultCode = 1, newRound = "", winner = "", draw = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }



        // GET: Home/GameOver/1
        [Route("Home/GameOver/{gameId}")]
        public ActionResult GameOver(int gameId)
        {

            //get game winner
            //Game game = _query.GetPlayerPoints();

            //clear moves table





            return View();
        }
    }
}