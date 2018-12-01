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
            if (!System.IO.File.Exists("WeatherDatabase.db"))
            {
                BuildTables();
            }
        }

        public static string Connection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;

        }
        public static void BuildTables()
        {
            SQLiteConnection db = GetConnection();
            const string DEFAULT_SCHEMA_TABLES = "CREATE TABLE Readings ( `ReadingID` INTEGER, `StationID` INTEGER NOT NULL, `StationName` TEXT NOT NULL, `ReadingTime` TEXT NOT NULL, `ReadingDate` TEXT NOT NULL, `ReadingDateTime` INTEGER NOT NULL, `ApparentTemperature` NUMERIC, `DeltaT` NUMERIC, `WindGustKmh` INTEGER, `WindGustKt` INTEGER, `ActualTemperature` NUMERIC, `DewPoint` NUMERIC, `PressureHpa` NUMERIC, `RainFallmm` TEXT, `RelativeHumidity` INTEGER, `BasicForecast` TEXT, `WindDirection` TEXT, `WindSpeedKmh` INTEGER, `WindSpeedKts` INTEGER);" +
            "CREATE TABLE Stations ( `Name` TEXT NOT NULL UNIQUE, `Identifier` INTEGER NOT NULL UNIQUE, `Lattitude` NUMERIC, `Longitude` NUMERIC, PRIMARY KEY(`Identifier`);";
            db.Execute(DEFAULT_SCHEMA_TABLES);
        }
        public static SQLiteConnection GetConnection()
        {

            return new SQLiteConnection(Connection("WeatherDatabase"));


        }
    }
}
