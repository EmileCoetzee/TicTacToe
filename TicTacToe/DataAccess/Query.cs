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

    }
}