using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDatabase
{
    public class Station
    {
        public string Name { get; set; }
        public int Identifier { get; set; }
        public float Lattitude { get; set; }
        public float Longitude { get; set; }
    }
}
