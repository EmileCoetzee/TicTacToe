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

    }

    public class BoardViewModel
    {
        public int Round { get; set; }

        public string PlayerName { get; set; }

    }
}