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
    public class ResultsController : Controller
    {
        Query _query;
        Ganss.XSS.HtmlSanitizer _htmlSanitizer;

        public ResultsController()
        {
            _query = new Query();
            _htmlSanitizer = new Ganss.XSS.HtmlSanitizer();
        }

        // GET: Results/Leaderboard
        public ActionResult Leaderboard()
        {
            List<Player> playerList = new List<Player>();

            playerList = _query.LoadScores();

            return View(playerList);
        }

        // GET: Results/GameHistory
        public ActionResult GameHistory()
        {
            List<GamePlayerViewModel> vmList = new List<GamePlayerViewModel>();

            List<Game> gameList = new List<Game>();

            gameList = _query.GetGamesCompleted();

            foreach (var game in gameList)
            {
                Player player1 = _query.GetPlayer(game.Player1Id);
                Player player2 = _query.GetPlayer(game.Player2Id);

                GamePlayerViewModel vm = new GamePlayerViewModel
                {
                    Game = game,
                    Player1 = player1,
                    Player2 = player2
                };

                vmList.Add(vm);
            }


            return View(vmList);
        }
    }
}