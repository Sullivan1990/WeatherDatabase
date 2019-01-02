using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Configuration;
using Dapper;

namespace WeatherDatabase
{
    static class DatabaseCon
    {   
        static DatabaseCon()
        {
            // if the Database file is missing, create it
            if (!System.IO.File.Exists("WeatherDatabase.db"))
            {
                Database.BuildTables();
                Functions newfunc = new Functions();
                newfunc.getSuburbzipHTTP();
            }
        }

        public static string Connection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;

        }
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(Connection("WeatherDatabase"));
        }


    }
}
