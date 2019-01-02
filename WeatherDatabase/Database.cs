using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace WeatherDatabase
{
    public class Database
    {
        //TODO Method to read in Suburb lat and lang from .csv file
            // filtering by QLD to suit the station's only being QLD stations

        //TODO Method to calculate the difference between each suburb lat and long
            // and compare to each stations lat and long
            // find the three closest stations based on lat and long diff

        //TODO Method to present the user a selection of the 3 closest stations
            // using this selection to build the first reading table
            // maybe even allowing for multiple picks, up to 3, creating 3 tables
        
        //TODO Set up the saving of these stations names and entered postcode
            // in the App.Config file
            // can use to check that the database contains matching tables

        //TODO Create a basic GUI for entering a postcode
            // GUI should allow for some basic testing queries
            // and testing of some graphing

        //TODO investigate htmlscraping vs using the JSON files for single suburb databases.

        public static void BuildTables()
        {
            SQLiteConnection db = DatabaseCon.GetConnection();
            const string DEFAULT_SCHEMA_TABLES = "CREATE TABLE 'Brisbane' ( `ReadingID` INTEGER PRIMARY KEY AUTOINCREMENT, `StationID` INTEGER, `StationName` TEXT, 'ReadingTimeIdent' INTEGER, 'ReadingYear' INTEGER, 'ReadingMonth' INTEGER, 'ReadingDay' INTEGER, 'ReadingTime' NUMERIC, `ApparentTemperature` NUMERIC, `DeltaT` NUMERIC, `WindGustKmh` INTEGER, `WindGustKt` INTEGER, `ActualTemperature` NUMERIC, `DewPoint` NUMERIC, `PressureHpa` NUMERIC, `RainFallmm` TEXT, `RelativeHumidity` INTEGER, `BasicForecast` TEXT, `WindDirection` TEXT, `WindSpeedKmh` INTEGER, `WindSpeedKts` INTEGER)";
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
                Logging.Log("Stations added to the database");
            }
            catch(Exception ex)
            {
                Logging.Log("ERROR", "Station insertion Error", ex.Message);
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
                trans.Commit();
                if (count == 0)
                {
                    Console.WriteLine("The database is up to date");
                    Logging.Log("The Database is up to date");
                }
                else
                {
                    Console.WriteLine(count + " Entries added to the database");
                    Logging.Log(count + " Entries added to the database");
                }
                Console.Read();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logging.Log("ERROR", "Reading Insertion Error", ex.Message); 
                trans.Rollback();
            }

        }
        public static void CheckTableExists()
        {
            SQLiteConnection db = DatabaseCon.GetConnection();
            string TableExists = "SELECT COUNT ('Brisbane') FROM sqlite_sequence";
            var Count = db.Query<int>(TableExists);
            if (Count.First() == 0)
            {
                string makeNewTable = $"CREATE TABLE 'Brisbane'( `ReadingID` INTEGER PRIMARY KEY AUTOINCREMENT, `StationID` INTEGER, `StationName` TEXT, 'ReadingTimeIdent' INTEGER, 'ReadingYear' INTEGER, 'ReadingMonth' INTEGER, 'ReadingDay' INTEGER, 'ReadingTime' INTEGER, `ApparentTemperature` NUMERIC, `DeltaT` NUMERIC, `WindGustKmh` INTEGER, `WindGustKt` INTEGER, `ActualTemperature` NUMERIC, `DewPoint` NUMERIC, `PressureHpa` NUMERIC, `RainFallmm` NUMERIC, `RelativeHumidity` INTEGER, `BasicForecast` TEXT, `WindDirection` TEXT, `WindSpeedKmh` INTEGER, `WindSpeedKts` INTEGER)";
                db.Execute(makeNewTable);
                Logging.Log("'Brisbane' Table created");
            }
        }
        public static int CheckTableEmpty(string TableName)
        {
            string checkempty = $"SELECT COUNT (*) FROM {TableName}";
            SQLiteConnection db = DatabaseCon.GetConnection();
            var Count = db.Query<int>(checkempty);
            db.Close();
            return Count.First();
        }
        public static void StationData(List<Station> newStations)
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
        public static long QueryLastReading()
        {
            SQLiteConnection db = DatabaseCon.GetConnection();
            db.Open();
            string querylast = "SELECT ReadingTimeIdent FROM Brisbane ORDER BY ReadingTimeIdent DESC LIMIT 1";
            long steve = db.QuerySingle<long>(querylast);
            return steve;
        }

        //public static void RebuildTable()
        //{
        //    try
        //    {

            
        //    RefinedReadingData ReadData = new RefinedReadingData();
        //    List<ReadingDatav2> WriteData = new List<ReadingDatav2>();
        //    string queryThisShit = "SELECT * FROM Readings";
            
        //    SQLiteConnection db = DatabaseCon.GetConnection();
        //    db.Open();
        //    List<RefinedReadingData> ReadingData = db.Query<RefinedReadingData>(queryThisShit).AsList();
        //    db.Close();
        //        foreach (RefinedReadingData Reading in ReadingData)
        //        {
        //            string datetimeNumber = "";
        //            ReadingDatav2 NewData = new ReadingDatav2();

        //            NewData.StationID = Reading.StationID;
        //            NewData.StationName = Reading.StationName;
        //            // NewData.ReadingDateTime = Reading.ReadingDateTime;

        //            datetimeNumber = Reading.ReadingTimeIdent.ToString();
        //            NewData.ReadingYear = Convert.ToInt16(datetimeNumber.Substring(0, 4));
        //            NewData.ReadingMonth = Convert.ToInt16(datetimeNumber.Substring(4, 2));
        //            NewData.ReadingDay = Convert.ToInt16(datetimeNumber.Substring(6, 2));
        //            // datetimeNumber.Substring
        //            if (datetimeNumber.Substring(10, 1).Equals("0"))
        //            {
        //                NewData.ReadingTime = Convert.ToDouble(datetimeNumber.Substring(8, 2));
        //            }
        //            else
        //            {
        //                NewData.ReadingTime = Convert.ToDouble(datetimeNumber.Substring(8, 2));
        //                NewData.ReadingTime += 0.5;
        //            }

        //            NewData.ReadingTimeIdent = Reading.ReadingTimeIdent;
        //            NewData.ApparentTemperature = Reading.ApparentTemperature;
        //            NewData.DeltaT = Reading.DeltaT;
        //            NewData.WindGustKmh = Reading.WindGustKmh;
        //            NewData.WindGustKt = Reading.WindGustKt;
        //            NewData.ActualTemperature = Reading.ActualTemperature;
        //            NewData.DewPoint = Reading.DewPoint;
        //            NewData.PressureHpa = Reading.PressureHpa;
        //            string[] Rainfallsplit = Reading.RainFallmm.Split('"');
        //            NewData.RainFallmm = Convert.ToDouble(Rainfallsplit[0]);
        //            NewData.RelativeHumidity = Reading.RelativeHumidity;
        //            NewData.BasicForecast = Reading.BasicForecast;
        //            NewData.WindDirection = Reading.WindDirection;
        //            NewData.WindSpeedKmh = Reading.WindSpeedKmh;
        //            NewData.WindSpeedKt = Reading.WindSpeedKt;


        //            WriteData.Add(NewData);
        //        }



        //        // Create new table


        //    foreach (ReadingDatav2 Reading in WriteData)
        //    {
        //        string insertLine = $"INSERT INTO Brisbane (StationID, StationName, ReadingTimeIdent, ReadingYear, ReadingMonth, ReadingDay, ReadingTime, ApparentTemperature, DeltaT, WindGustKmh, WindGustKt, ActualTemperature, DewPoint, PressureHpa, RainFallmm, RelativeHumidity, BasicForecast, WindDirection, WindSpeedKmh, WindSpeedKts)" +
        //                $" VALUES ({Reading.StationID}, '{Reading.StationName}', {Reading.ReadingTimeIdent}, {Reading.ReadingYear}, {Reading.ReadingMonth}, {Reading.ReadingDay}, {Reading.ReadingTime}, {Reading.ApparentTemperature}, {Reading.DeltaT}, {Reading.WindGustKmh}, {Reading.WindGustKt}, {Reading.ActualTemperature}, {Reading.DewPoint}, {Reading.PressureHpa}, '{Reading.RainFallmm}', {Reading.RelativeHumidity}, '{Reading.BasicForecast}', '{Reading.WindDirection}', {Reading.WindSpeedKmh}, {Reading.WindSpeedKt})";
        //        db.Execute(insertLine);
        //    }
        //    db.Close();
        //        // Create new table
        //        // Write the list to the table
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }   
}
