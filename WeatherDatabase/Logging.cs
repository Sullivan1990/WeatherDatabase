using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDatabase
{
    public static class Logging
    {
        public static void Log (string message)
        {
            using (System.IO.StreamWriter file =
              new System.IO.StreamWriter(@"LogINFO.txt", true))
            {
                try
                {
                    string LDate = DateTime.Now.ToShortDateString();
                    string LTime = DateTime.Now.ToString("h:mm:ss tt");

                    string logMessage = $"{LDate}, {LTime}, INFO: {message}";
                    file.WriteLine(logMessage);

                }
                catch
                {
                    Console.WriteLine("Log Write Error");
                }
                finally
                {
                    // not really necessary using 'using' for the streamwriter
                    file.Close();
                }
            }
        }
        public static void Log(string type, string message)
        {
            using (System.IO.StreamWriter file =
              new System.IO.StreamWriter(@"LogINFO.txt", true))
            {
                try
                {
                    string LDate = DateTime.Now.ToShortDateString();
                    string LTime = DateTime.Now.ToString("h:mm:ss tt");

                    string logMessage = $"{LDate}, {LTime}, {type}: {message}";
                    file.WriteLine(logMessage);
                    Console.WriteLine("There has been a problem, please check the log for details");

                }
                catch
                {
                    Console.WriteLine("Log Write Error");
                }
                finally
                {
                    // not really necessary using 'using' for the streamwriter
                    file.Close();
                }
            }
        }
        public static void Log(string type, string description, string message)
        {
            using (System.IO.StreamWriter file =
              new System.IO.StreamWriter(@"LogINFO.txt", true))
            {
                try
                {
                    string LDate = DateTime.Now.ToShortDateString();
                    string LTime = DateTime.Now.ToString("h:mm:ss tt");

                    string logMessage = $"{LDate}, {LTime}, {type}:, {description}, {message}";
                    file.WriteLine(logMessage);
                    Console.WriteLine("There has been a problem, please check the log for details");

                }
                catch
                {
                    Console.WriteLine("Log Write Error");
                }
                finally
                {
                    // not really necessary using 'using' for the streamwriter
                    file.Close();
                }
            }
        }
    }
}
