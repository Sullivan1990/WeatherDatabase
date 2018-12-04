using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDatabase
{
    public class RefinedReadingData
    {
        // public int SortOrder { get; set; }                  // Irrelevant
        public int StationID { get; set; }
        public string StationName { get; set; }
        public string ReadingTime { get; set; }
        public string ReadingDate { get; set; }
        public DateTime ReadingDateTime { get; set; }
        public long ReadingTimeIdent { get; set; }
        public double Stationlattitude { get; set; }        //GOTO Stations table
        public double Stationlongitude { get; set; }        //GOTO Stations table
        public float ApparentTemperature { get; set; }
        public float DeltaT { get; set; }
        public int WindGustKmh { get; set; }
        public int WindGustKt { get; set; }
        public float ActualTemperature { get; set; }
        public float DewPoint { get; set; }
        public float PressureHpa { get; set; }
        public string RainFallmm { get; set; }
        public int RelativeHumidity { get; set; }
        public string BasicForecast { get; set; }
        public string WindDirection { get; set; }
        public int WindSpeedKmh { get; set; }
        public int WindSpeedKt { get; set; }
    }
}
