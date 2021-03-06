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
                INTO Games (Player1Id, Player2Id, Date) 
                OUTPUT INSERTED.Id 
                VALUES (@Player1Id, @Player2Id, @Date)";

                string sqlMoves = @"INSERT 
                INTO Moves (GamesId) 
                VALUES (@GameId)";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    int gameId = cnn.QuerySingle<int>(sql, new { Player1Id = player1Id, Player2Id = player2Id, Date = DateTime.Now.ToString("dd-MM-yy HH:mm:ss") });

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

        // update game completed date
        // params = 0. gameId 
        public int UpdateGameTimeStamp(int gameId)
        {
            try
            {
                var sql = @"UPDATE Games SET
                   Date = @Date
                   WHERE Id = @Id;";

                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    cnn.Execute(sql, new { Date = DateTime.Now.ToString("dd-MM-yy HH:mm:ss"), Id = gameId });
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


        // get game detail
        // params = 0. gameId
        // return game object
        public Game GetGameDetail(int gameId)
        {

            string sql = @"SELECT * 
                FROM Games 
                WHERE Id = @Id;";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                   return cnn.QuerySingleOrDefault<Game>(sql, new { Id = gameId });

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        // get games that are not complete
        // params = 0. player1Id, 1.player2Id
        // return game object
        public Game CheckForSavedGames(int player1Id, int player2Id)
        {
            string sql = @"SELECT TOP (1) * 
                FROM Games 
                WHERE HighestRoundCompleted < 3
                AND Player1Id = @Player1Id 
                AND Player2Id = @Player2Id";


            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    return cnn.QuerySingleOrDefault<Game>(sql, new { Player1Id = player1Id, Player2Id = player2Id });
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        // get games that are complete
        // return list of game object
        public List<Game> GetGamesCompleted()
        {

            string sql = @"SELECT * FROM Games 
                WHERE HighestRoundCompleted = 3 
                ORDER BY 'Date' DESC;";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    return cnn.Query<Game>(sql).ToList();

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        // get Moves list
        // params = 0. gameId
        // return Move object
        public Move GetMoves(int gameId)
        {
            Move move = new Move();

            try
            {

                var sql = "SELECT * FROM Moves WHERE GamesId = @GameId";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    return cnn.QuerySingleOrDefault<Move>(sql, new { GameId = gameId });
                }
            }
            catch (Exception)
            {
                return move;
            }

        }


        // get player detail
        // params = 0. playerId
        // return player object
        public Player GetPlayerDetail(int playerId)
        {

            string sql = @"SELECT * 
                FROM Players 
                WHERE Id = @Id;";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    return cnn.QuerySingleOrDefault<Player>(sql, new { Id = playerId });

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        // get player detail
        // params = 0. player name
        // return player object
        public Player GetPlayerDetail(string playerName)
        {

            string sql = @"SELECT TOP(1) * 
                FROM Players 
                WHERE Name = @Name;";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    return cnn.QuerySingleOrDefault<Player>(sql, new { Name = playerName });

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }


        // delete moves for particular game
        // params = 0. gameId
        public int DeleteMoves(int gameId)
        {
            var sql = @"DELETE FROM 
                    Moves 
                    WHERE GamesId = @Id;";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    cnn.Execute(sql, new { Id = gameId });

                }
                catch (Exception)
                {
                    return 0;
                }

            }

            return 1;
        }


        // update player total points
        // params = 0. player object
        public int UpdatePlayerPoints(Player player)
        {
            try
            {

                var sql = @"UPDATE Players SET 
                    TotalPoints = @Points
                    WHERE Id = @Id;";


                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    cnn.Execute(sql, new { Id = player.Id, Points = player.TotalPoints });
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 1;
        }

        // get leaderboard scores
        // return list of player object
        public List<Player> LoadScores()
        {

            string sql = @"SELECT * 
                FROM Players WHERE TotalPoints > 0
                ORDER BY TotalPoints DESC";

            using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
            {
                try
                {
                    return cnn.Query<Player>(sql).ToList();

                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

    }
}