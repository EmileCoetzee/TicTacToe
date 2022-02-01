using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToe.BusinessLogic;
using TicTacToe.DataAccess;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class GameController : Controller
    {
        Query _query;
        Ganss.XSS.HtmlSanitizer _htmlSanitizer;

        public GameController()
        {
            _query = new Query();
            _htmlSanitizer = new Ganss.XSS.HtmlSanitizer();
        }

        // GET: Game/Start 
        public ActionResult Start()
        {
            return View();
        }

        // GET: Game/Load 
        public ActionResult Load()
        {
            LoadGameViewModel vm = new LoadGameViewModel();

            vm.Player1 = new Player();
            vm.Player2 = new Player();

            return View(vm);
        }

        // POST: Game/LoadGame
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LoadGame(LoadGameViewModel model)
        {

            model.Player1.Name = _htmlSanitizer.Sanitize(model.Player1.Name);
            model.Player2.Name = _htmlSanitizer.Sanitize(model.Player2.Name);

            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Error, some data is missing.  Please ensure that all fields are entered.");

                //get player details
                model.Player1 = _query.GetPlayerDetail(model.Player1.Name);
                model.Player2 = _query.GetPlayerDetail(model.Player2.Name);


                if (model.Player1 is null || model.Player2 is null)
                    throw new Exception("A saved game could not be found for these players.");

                //check for any games that don't have a highestRound completed of 3
                Game game = _query.CheckForSavedGames(model.Player1.Id, model.Player2.Id);

                if (game is null)
                    throw new Exception("A saved game could not be found for these players.");

                return Json(new { data = "Starting Game...", resultCode = 1, player1Id = model.Player1.Id, player2Id = model.Player2.Id, gameId = game.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Game/GetPlayer1Form (Partial)
        public PartialViewResult GetPlayer1Form()
        {
            Player player = new Player();

            player.isPlayer1 = true;

            PlayerStartViewModel vm = new PlayerStartViewModel();
            vm.Player = player;

            return PartialView(vm);
        }

        // GET: Game/GetPlayer2Form (Partial)
        public PartialViewResult GetPlayer2Form()
        {
            Player player = new Player();

            player.isPlayer1 = false;

            PlayerStartViewModel vm = new PlayerStartViewModel();
            vm.Player = player;

            return PartialView(vm);
        }

        // POST: Game/AddPlayer
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
                    redirectURL = "/Game/Play/" + model.Player1Id + "/" + playerId + "/" + 0;


                return Json(new { data = redirectURL, resultCode = 1, player1Id = playerId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = ex.Message.ToString(), resultCode = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Game/Play/1/1/1
        [Route("Game/Play/{player1Id}/{player2Id}/{gameId}")]
        public ActionResult Play(int player1Id, int player2Id, int gameId)
        {
            Game game = new Game();
            Move move = new Move();

            int currentPlayer = 1;


            //create new game
            if (gameId == 0)
            {
                gameId = _query.CreateGame(player1Id, player2Id);

            }
            else
            {
                //get current round
                game = _query.GetGameDetail(gameId);

                //get moves list
                move = _query.GetMoves(gameId);


                int p1MovesCount = 0;
                int p2MovesCount = 0;

                int[] moves = { move.B1, move.B2, move.B3, move.B4, move.B5, move.B6, move.B7, move.B8, move.B9 };

                for (int i = 0; i < moves.Length; i++)
                {
                    switch (moves[i])
                    {
                        case 1:
                            p1MovesCount++;
                            break;
                        case 2:
                            p2MovesCount++;
                            break;
                    }
                }

                //determine who's turn it is
                if (p1MovesCount > p2MovesCount)
                {
                    currentPlayer = 2;
                }

            }


            Player player1 = _query.GetPlayer(player1Id);
            player1.isPlayer1 = true;

            Player player2 = _query.GetPlayer(player2Id);

            ViewBag.CurrentPlayer = currentPlayer;
            ViewBag.GameId = gameId;
            ViewBag.CurrentPlayerName = player1.Name;

            if (currentPlayer == 2)
            {
                ViewBag.CurrentPlayerName = player2.Name;
            }


            PlayerPlayViewModel playerVM = new PlayerPlayViewModel();

            playerVM.Player1 = player1;
            playerVM.Player2 = player2;
            playerVM.Game = game;
            playerVM.Move = move;

            return View(playerVM);
        }

        // POST: Game/SaveMove/1/1/1/1
        [HttpPost]
        [Route("Game/SaveMove/{gameId}/{currentRound}/{currentPlayer}/{blockId}")]
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

                int winner = Calculations.GetWinner(moves, currentPlayer);


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
                else if (!moves.Contains(0))
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

        // GET: Game/GameOver/1
        [Route("Game/GameOver/{gameId}")]
        public ActionResult GameOver(int gameId)
        {
            //delete moves
            _query.DeleteMoves(gameId);

            //update timestamp
            _query.UpdateGameTimeStamp(gameId);

            //get game details
            GamePlayerViewModel vm = new GamePlayerViewModel();

            vm.Game = _query.GetGameDetail(gameId);
            vm.Player1 = _query.GetPlayerDetail(vm.Game.Player1Id);
            vm.Player2 = _query.GetPlayerDetail(vm.Game.Player2Id);

            vm.Player1.TotalPoints = vm.Player1.TotalPoints + vm.Game.Player1Points;
            vm.Player2.TotalPoints = vm.Player2.TotalPoints + vm.Game.Player2Points;


            //update player total points
            _query.UpdatePlayerPoints(vm.Player1);
            _query.UpdatePlayerPoints(vm.Player2);

            return View(vm);
        }

        // GET: Game/GetBoard/1/name (Partial)
        [Route("Game/GetBoard/{currentRound}/{currentPlayerName}")]
        public PartialViewResult GetBoard(int currentRound, string currentPlayerName)
        {
            currentPlayerName = _htmlSanitizer.Sanitize(currentPlayerName);

            BoardViewModel vm = new BoardViewModel();

            vm.Round = currentRound;
            vm.PlayerName = currentPlayerName;

            return PartialView(vm);

        }
    }
}