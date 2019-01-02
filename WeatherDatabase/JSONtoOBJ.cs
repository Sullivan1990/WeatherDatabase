using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;


namespace WeatherDatabase
{
    public static class JSONtoOBJ
    {
        public static List<RawReadingData> Objectify(string fileName)
        {
            try
            {
                string loadText = File.ReadAllText(fileName);

                // Read the entire document into a JObject
                JObject EntireBOMdocument = JObject.Parse(loadText);


                // Pick out the section we want, here it is "data" under the parent of "observations"
                // and saves all of the seperate instances in a list 
                List<JToken> seperateReadings = EntireBOMdocument["observations"]["data"].Children().ToList();


                // Instantiated list of ActalObj
                List<RawReadingData> readingObjects = new List<RawReadingData>();
                // For each Token seperated item in the string
                foreach (JToken readings in seperateReadings)
                {
                    // parse the line into an object
                    RawReadingData singleReading = readings.ToObject<RawReadingData>();
                    // Add the newly created object to a list of objects
                    readingObjects.Add(singleReading);
                }
                // for each newly created object in the list of objects
                foreach (RawReadingData displayReading in readingObjects)
                {
                    // display the air temp + Humidity
                    //Console.WriteLine(displayReading.air_temp + "," + displayReading.apparent_t);
                }
                return readingObjects;
            }
            catch (Exception ex)
            {
                Logging.Log("ERROR", "JSON to OBJ ERROR", ex.Message);
                throw;
            }
        }
    }

}
