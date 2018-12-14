using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Globalization;


namespace WeatherDatabase
{
    class Program
    {


        static void Main(string[] args)
        {
            Functions Application = new Functions();

            Application.FileCheck();
           // Application.DisplayStations();
            
            // Console.WriteLine("Please enter a station you would like to record:\n\n");

            // Originally .ListConvert(Console.ReadLine())

        }


    }
}
