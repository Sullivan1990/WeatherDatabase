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
            string querylast = "SELECT ReadingTimeIdent FROM Readings ORDER BY ReadingTimeIdent DESC LIMIT 1";
            long steve = db.QuerySingle<long>(querylast);
            return steve;
        }
    }   
}
