using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int Player1Id { get; set; }

        public int Player2Id { get; set; }

        public int Player1Points { get; set; }

        public int Player2Points { get; set; }

        public int HighestRoundCompleted { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public Player Player { get; set; }

    }

    public class BoardViewModel
    {
        public int Round { get; set; }

        public string PlayerName { get; set; }

    }

    public class GamePlayerViewModel
    {
        public Game Game { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }
    }

    public class LoadGameViewModel
    {
        public Player Player1 { get; set; }

        public Player Player2 { get; set; }
    }
}