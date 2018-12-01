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
            if (CheckTableEmpty("Stations")==0)
            {
                SQLiteConnection db = DatabaseCon.GetConnection();
                db.Open();
                SQLiteTransaction trans = db.BeginTransaction();
                try
                {
                    for (int i = 0; i < newStations.Count; i++)
                    {
                        string adddataQuery = $"INSERT INTO Stations (Name, Identifier, Lattitude, Longitude) VALUES ({newStations[i].Name}, {newStations[i].Identifier}, {newStations[i].Lattitude}, {newStations[i].Longitude})";
                        db.Execute(adddataQuery, transaction: trans);
                    }
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                }

            }
        }
    }   
}
