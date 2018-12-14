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
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(Connection("WeatherDatabase"));
        }
        public static void BuildTables()
        {
            SQLiteConnection db = GetConnection();
            const string DEFAULT_SCHEMA_TABLES = "CREATE TABLE 'Brisbane' ( `ReadingID` INTEGER PRIMARY KEY AUTOINCREMENT, `StationID` INTEGER, `StationName` TEXT, 'ReadingTimeIdent' INTEGER, `ApparentTemperature` NUMERIC, `DeltaT` NUMERIC, `WindGustKmh` INTEGER, `WindGustKt` INTEGER, `ActualTemperature` NUMERIC, `DewPoint` NUMERIC, `PressureHpa` NUMERIC, `RainFallmm` TEXT, `RelativeHumidity` INTEGER, `BasicForecast` TEXT, `WindDirection` TEXT, `WindSpeedKmh` INTEGER, `WindSpeedKts` INTEGER)";
            const string StationTable = "CREATE TABLE 'Stations' ( `Name` TEXT NOT NULL UNIQUE, `Identifier` INTEGER PRIMARY KEY, `Lattitude` NUMERIC, `Longitude` NUMERIC)"; // , PRIMARY KEY(`Identifier`)";
            db.Execute(DEFAULT_SCHEMA_TABLES);
            db.Execute(StationTable);
        }
        public static void BuildStations(List<Station> newStations)
        {
            SQLiteConnection db = DatabaseCon.GetConnection();
            db.Open();
            SQLiteTransaction trans = db.BeginTransaction();
            try
            {
                for (int i = 0; i < newStations.Count; i++)
                {
                    string adddataQuery = $"INSERT INTO Stations (Name, Identifier, Lattitude, Longitude) VALUES ('{newStations[i].Name}', {newStations[i].Identifier}, 1, 1)";
                    db.Execute(adddataQuery, transaction: trans);
                    //trans.Commit();
                }
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
            }
        }

        public static void BuildReadings(List<ReadingDatav2> Buildlist)
        {
            SQLiteConnection db = DatabaseCon.GetConnection();
            db.Open();
            SQLiteTransaction trans = db.BeginTransaction();
            try
            {
                long LastDate;
                if (Database.CheckTableEmpty("Brisbane") == 0)
                {
                    LastDate = 0;
                }
                else
                {
                    LastDate = Database.QueryLastReading();
                }
                
                int count = 0;
                for (int i = Buildlist.Count - 1; i > 0; i--)
                {
                    ReadingDatav2 newData = new ReadingDatav2();
                    newData = Buildlist[i];
                    if (newData.ReadingTimeIdent > LastDate || LastDate == 0)
                    {
                        string insertLine = $"INSERT INTO Brisbane (StationID, StationName, ReadingTimeIdent, ReadingYear, ReadingMonth, ReadingDay, ReadingTime, ApparentTemperature, DeltaT, WindGustKmh, WindGustKt, ActualTemperature, DewPoint, PressureHpa, RainFallmm, RelativeHumidity, BasicForecast, WindDirection, WindSpeedKmh, WindSpeedKts)" +
                                $" VALUES ({newData.StationID}, '{newData.StationName}', {newData.ReadingTimeIdent}, {newData.ReadingYear}, {newData.ReadingMonth}, {newData.ReadingDay}, {newData.ReadingTime}, {newData.ApparentTemperature}, {newData.DeltaT}, {newData.WindGustKmh}, {newData.WindGustKt}, {newData.ActualTemperature}, {newData.DewPoint}, {newData.PressureHpa}, '{newData.RainFallmm}', {newData.RelativeHumidity}, '{newData.BasicForecast}', '{newData.WindDirection}', {newData.WindSpeedKmh}, {newData.WindSpeedKt})";
                        db.Execute(insertLine);
                        count++;
                    }

                }
                Console.WriteLine(count);
                Console.Read();
                trans.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                trans.Rollback();
            }

        }


    }
}
