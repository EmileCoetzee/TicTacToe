using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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

                using (IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["Staging"].ConnectionString))
                {
                    return cnn.QuerySingle<int>(sql, new { Player1Id = player1Id, Player2Id = player2Id });
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}