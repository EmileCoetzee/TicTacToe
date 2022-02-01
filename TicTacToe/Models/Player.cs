using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Display(Name="Player Name")]
        public string Name { get; set; }

        [Display(Name = "Score")]
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

    public class PlayerPlayViewModel
    {
        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public Game Game { get; set; }

        public Move Move { get; set; }

    }
}