using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Move
    {
        public int Id { get; set; }

        public int GamesId { get; set; }

        public int B1 { get; set; } = 0;

        public int B2 { get; set; } = 0;

        public int B3 { get; set; } = 0;

        public int B4 { get; set; } = 0;

        public int B5 { get; set; } = 0;

        public int B6 { get; set; } = 0;

        public int B7 { get; set; } = 0;

        public int B8 { get; set; } = 0;

        public int B9 { get; set; } = 0;

    }
}