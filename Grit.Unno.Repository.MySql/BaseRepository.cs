using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Unno.Repository.MySql
{
    public class BaseRepository
    {
        //private static string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
        protected SqlOptions _options;
        public MySqlConnection OpenConnection()
        {
            if (_options == null)
            {
                _options = new SqlOptions(System.Configuration.ConfigurationManager.ConnectionStrings["mysql"].ConnectionString);
            }
            MySqlConnection connection = new MySqlConnection(_options.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}
