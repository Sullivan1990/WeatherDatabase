using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDatabase
{
    public class RawReadingData
    {
        public int sort_order { get; set; }
        public int wmo { get; set; }
        public string name { get; set; }
        public string history_product { get; set; }
        public string local_date_time { get; set; }
        public string local_date_time_full { get; set; }
        public string aifstime_utc { get; set; }
        public string aifstime_local { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string apparent_t { get; set; }
        public string cloud { get; set; }
        public string cloud_base_m { get; set; }
        public string cloud_oktas { get; set; }
        public string cloud_type_id { get; set; }
        public string cloud_type { get; set; }
        public string delta_t { get; set; }
        public string gust_kmh { get; set; }
        public string gust_kt { get; set; }
        public string air_temp { get; set; }
        public string dewpt { get; set; }
        public string press { get; set; }
        public string press_qnh { get; set; }
        public string press_msl { get; set; }
        public string press_tend { get; set; }
        public string rain_trace { get; set; }
        public string rel_hum { get; set; }
        public string sea_state { get; set; }
        public string swell_dir_worded { get; set; }
        public string swell_height { get; set; }
        public string swell_period { get; set; }
        public string vis_km { get; set; }
        public string weather { get; set; }
        public string wind_dir { get; set; }
        public string wind_spd_kmh { get; set; }
        public string wind_spd_kt { get; set; }
    }
}
