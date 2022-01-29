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

		// save an object and return inserted Id
		// params = 0. sql statement 1. object
		public int CheckIfPlayerExists(String playerName)
		{
            int playerId = 0;

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
				playerId =  cnn.QuerySingleOrDefault<int>(sql, new { Name = playerName });

                if (playerId != 0)
                    return playerId;

                return cnn.QuerySingle<int>(sqlInsert, new { Name = playerName });
            }
		}

	}
}