using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TicTacToe.Models;

namespace TicTacToe.DataAccess
{
	public class Query
	{

		public List<T> LoadData<T>(String sql)
		{
			using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
			{
				return cnn.Query<T>(sql).ToList();
			}
		}

		// check if player exists
        // if not, create new
		// params = 0. player name
        // return playerId
		public int CheckIfPlayerExists(String playerName)
		{
            int playerId = 0;

            try
            {

                string sql = @"SELECT *
				FROM
				Players 
				WHERE
				Name = @Name";

                string sqlInsert = @"INSERT 
                INTO Players (Name) 
                OUTPUT INSERTED.Id 
                VALUES (@Name)";

                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    playerId = cnn.QuerySingleOrDefault<int>(sql, new { Name = playerName });

                    if (playerId != 0)
                        return playerId;

                    return cnn.QuerySingle<int>(sqlInsert, new { Name = playerName });
                }
            }
            catch (Exception)
            {
                return playerId;
            }
		}


        // create new game object
        // params = 0. player1Id, 1. player2Id
        // return GameId
        public int CreateGame(int player1Id, int player2Id)
        {
            try
            {
                string sql = @"INSERT 
                INTO Games (Player1Id, Player2Id) 
                OUTPUT INSERTED.Id 
                VALUES (@Player1Id, @Player2Id)";

                string sqlMoves = @"INSERT 
                INTO Moves (GamesId) 
                VALUES (@GameId)";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    int gameId = cnn.QuerySingle<int>(sql, new { Player1Id = player1Id, Player2Id = player2Id });

                    if (gameId != 0)
                        cnn.Execute(sqlMoves, new { GameId = gameId });

                    return gameId;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // update Moves
        // params = 0. gameId 1. player number (1 or 2)
        // return Move object
        public Move UpdateMoves(int gameId, int currentPlayer, int blockId)
        {
            try
            {
                var sql = "UPDATE Moves SET " +
               "B" + blockId + " = @CurrentPlayer " +
                   "WHERE GamesId = @GameId;";


                var sqlSelect = "SELECT * FROM Moves WHERE GamesId = @GameId";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    cnn.Execute(sql, new {
                        CurrentPlayer = currentPlayer,
                        GameId = gameId
                    });

                    return cnn.QuerySingleOrDefault<Move>(sqlSelect, new { GameId = gameId });

                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}