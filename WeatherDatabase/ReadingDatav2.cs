using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDatabase
{
    class ReadingDatav2
    {
        public int StationID { get; set; }
        public string StationName { get; set; }

        public double ReadingTime { get; set; }

        public int ReadingDay { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingYear { get; set; }

        // public DateTime ReadingDateTime { get; set; }
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
        public double RainFallmm { get; set; }
        public int RelativeHumidity { get; set; }
        public string BasicForecast { get; set; }
        public string WindDirection { get; set; }
        public int WindSpeedKmh { get; set; }
        public int WindSpeedKt { get; set; }


    }
}
