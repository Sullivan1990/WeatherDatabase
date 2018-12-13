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
        public static void FirstRun()
        {

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
