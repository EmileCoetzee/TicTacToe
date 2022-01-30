using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TotalPoints { get; set; }

        public bool isPlayer1 { get; set; }

        public int Player1Id { get; set; }
    }

    public class PlayerStartViewModel
    {
        public Player Player { get; set; }

        public int Player1Id { get; set; }

        public int Player2Id { get; set; }
    }
}