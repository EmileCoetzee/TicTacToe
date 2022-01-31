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

        // get player name
        // params = 0. playerId
        public Player GetPlayer(int playerId)
        {
            Player player = new Player();

            try
            {
                string sql = @"SELECT *
				FROM
				Players 
				WHERE
				Id = @Id";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    player = cnn.QuerySingleOrDefault<Player>(sql, new { Id = playerId });
                }
            }
            catch (Exception)
            {
                return player;
            }

            return player;
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

        // clear Moves
        // params = 0. gameId 
        // return success/failure
        public int ClearMoves(int gameId)
        {
            try
            {
                var sql = @"UPDATE Moves SET 
                B1 = null,
                B2 = null,
                B3 = null,
                B4 = null,
                B5 = null,
                B6 = null,
                B7 = null,
                B8 = null,
                B9 = null
                   WHERE GamesId = @GameId;";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    cnn.Execute(sql, new { GameId = gameId });
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }

        // update Player score
        // params = 0. gameId, 1.player number, 2.points
        // return success/failure
        public int UpdateScore(int gameId, int currentPlayer, int points)
        {
            try
            {
                var sql = "SELECT * FROM Games WHERE Id = @Id";

                var sqlUpdate = @"UPDATE Games SET 
                    Player1Points = @Points
                    WHERE Id = @Id;";


                int updatedPoints = 0;

                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {

                    Game game = cnn.QuerySingleOrDefault<Game>(sql, new { Id = gameId });

                    if (game != null)
                    {
                        if (currentPlayer == 1)
                        {
                            updatedPoints = game.Player1Points + points;
                        }
                        else
                        {
                            updatedPoints = game.Player2Points + points;

                            sqlUpdate = @"UPDATE Games SET 
                            Player2Points = @Points
                            WHERE Id = @Id;";   
                        }
                    }

                    cnn.Execute(sqlUpdate, new { Id = gameId, Points = updatedPoints });
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }

        // update highest round completed
        // params = 0. gameId, 1. last round completed
        // return success/failure
        public int UpdateHighestRoundCompleted(int gameId, int currentRound)
        {
            try
            {

                var sql = @"UPDATE Games SET 
                    HighestRoundCompleted = @Round
                    WHERE Id = @Id;";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    cnn.Execute(sql, new { Id = gameId, Round = currentRound });
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }


    }
}