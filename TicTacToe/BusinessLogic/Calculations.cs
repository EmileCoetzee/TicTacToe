using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicTacToe.Models;

namespace TicTacToe.BusinessLogic
{
    public class Calculations
    {
        //check if winner
        //params 0.move object, 1.current player
        //returns winner, if any
        public static int GetWinner(int[] moves, int currentPlayer)
        {
            
            int winner = 0;

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

            return winner;
        }
    }
}