﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Net;


namespace WeatherDatabase
{
    class Functions
    {
        const string tempArchivePath = "C:\\New Folder\\";
        string tempArchiveName = $"C:\\New Folder\\temp{DateTime.Now.DayOfWeek.ToString()}.tgz";
        const string extractPath = tempArchivePath + "Temp\\";
        List<Station> stationlist = new List<Station>();
        List<ReadingDatav2> BetterList = new List<ReadingDatav2>();

        public void FolderCheck()
        {
            bool exists = Directory.Exists(extractPath);

            if (!exists)
            {
                Directory.CreateDirectory(extractPath);
                Console.WriteLine("Creating necessary Folders");
               
            }

               
            
        }
        public void ExtractTGZ(String gzArchiveName, String destFolder)
        {
            Console.WriteLine("Extracting Files...........");
            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
            // Console.WriteLine("Press enter to smash the screen with Weather Stations");
            // Console.ReadKey();
        }
        public bool FTPDownload()
        {

            try
            {
                Console.WriteLine("Downloading Files...........");
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp2.bom.gov.au/anon/gen/fwo/IDQ60910.tgz");
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream file = File.Create(tempArchiveName);
                byte[] buffer = new byte[32 * 1024];
                int read;

                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, read);
                }

                file.Close();
                responseStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public void Folderactions()
        {
            /*TODO Read the Lattitude and Longitude data from the .AXF file
                   Write the list of objects into the database
                   Test SQL is working as it should
                   and test the Database is being created
            */
            FolderCheck();
            string[] Data = Directory.GetFiles(tempArchivePath + "Temp\\", "*.axf");
            for (int x = 0; x < Data.Length; x++)
            {
                string[] lines = File.ReadAllLines(Data[x]);
                const int neededLines = 15;
                string[] lines2 = new string[16];
                int i = 0;
                foreach (var s in File.ReadLines(Data[x]))
                {
                    lines2[i] = s;
                    if (i == neededLines)
                    {
                        Station newStation = new Station();
                        string[] splitstring = lines2[10].Split('"');
                        string name = splitstring[1];
                        newStation.Name = splitstring[1];

                        splitstring = lines2[11].Split('"');
                        string Identification = splitstring[1];
                        
                        if(!(splitstring[1].Equals("QLD")))
                        {
                            newStation.Identifier = Convert.ToInt32(splitstring[1]);
                            stationlist.Add(newStation);
                        }
                        
                        break; // this just works, Don't remove it
                    }
                    i++;
                }
            }
            ListConvert("Brisbane");

        }
        public bool IsDirectoryEmpty()
        {
            return !Directory.EnumerateFileSystemEntries(extractPath).Any();
        }

        public void StationLocation (List<RawReadingData> RawList)
        {
            foreach (Station station in stationlist)
            {
                foreach(RawReadingData reading in RawList)
                {
                    if (station.Identifier == Convert.ToInt32(reading.wmo))
                    {
                        station.Lattitude = float.Parse(reading.lat);
                        station.Longitude = float.Parse(reading.lon);
                        // Console.WriteLine($"{station.Name}, {station.Lattitude}, {station.Longitude}");
                        break;
                    }
                }
            }
        }
        public List<ReadingDatav2> MessyConversion(List<RawReadingData> RawList)
        {
                     
            foreach (RawReadingData reading in RawList)
            {
                ReadingDatav2 BetterReading = new ReadingDatav2();
                string datetimeNumber = "";
                //BetterReading.SortOrder = Convert.ToInt32(reading.sort_order);
                BetterReading.StationID = Convert.ToInt32(reading.wmo);
                BetterReading.StationName = reading.name;
                //BetterReading.ReadingDateTime = DateTime.ParseExact(reading.aifstime_local, "yyyyMMddHHmmss", CultureInfo.InvariantCulture); //20181130213000
                //BetterReading.ReadingDate = BetterReading.ReadingDateTime.ToShortDateString();
                //BetterReading.ReadingTime = BetterReading.ReadingDateTime.ToShortTimeString();
                BetterReading.ReadingTimeIdent = Convert.ToInt64(reading.aifstime_local);
                datetimeNumber = BetterReading.ReadingTimeIdent.ToString();
                BetterReading.ReadingYear = Convert.ToInt16(datetimeNumber.Substring(0, 4));
                BetterReading.ReadingMonth = Convert.ToInt16(datetimeNumber.Substring(4, 2));
                BetterReading.ReadingDay = Convert.ToInt16(datetimeNumber.Substring(6, 2));
                // datetimeNumber.Substring
                if (datetimeNumber.Substring(10, 1).Equals("0"))
                {
                    BetterReading.ReadingTime = Convert.ToDouble(datetimeNumber.Substring(8, 2));
                }
                else
                {
                    BetterReading.ReadingTime = Convert.ToDouble(datetimeNumber.Substring(8, 2));
                    BetterReading.ReadingTime += 0.5;
                }
                //BetterReading.Stationlattitude = Convert.ToDouble(reading.lat);
                //BetterReading.Stationlongitude = Convert.ToDouble(reading.lon);
                if (reading.apparent_t == null || reading.apparent_t.Equals("")) { BetterReading.ApparentTemperature = 0.0f; }
                else { BetterReading.ApparentTemperature = float.Parse(reading.apparent_t); }
                if (reading.delta_t == null || reading.delta_t.Equals("")) { BetterReading.DeltaT = 0.0f; }
                else { BetterReading.DeltaT = float.Parse(reading.delta_t); }
                BetterReading.WindGustKmh = Convert.ToInt32(reading.gust_kmh);
                BetterReading.WindGustKt = Convert.ToInt32(reading.gust_kt);
                if (reading.air_temp == null || reading.air_temp.Equals("")) { BetterReading.ActualTemperature = 0.0f; }
                else { BetterReading.ActualTemperature = float.Parse(reading.air_temp); }
                if (reading.dewpt == null || reading.dewpt.Equals("")) { BetterReading.DewPoint = 0.0f; }
                else { BetterReading.DewPoint = float.Parse(reading.dewpt); }
                if (reading.press == null || reading.press.Equals("")) { BetterReading.PressureHpa = 0.0f; }
                else { BetterReading.PressureHpa = float.Parse(reading.press); }

                string[] Rainfallsplit = reading.rain_trace.Split('"');
                BetterReading.RainFallmm = Convert.ToDouble(Rainfallsplit[0]);
                BetterReading.RelativeHumidity = Convert.ToInt32(reading.rel_hum);
                BetterReading.BasicForecast = reading.weather;
                BetterReading.WindDirection = reading.wind_dir;
                BetterReading.WindSpeedKmh = Convert.ToInt32(reading.wind_spd_kmh);
                BetterReading.WindSpeedKt = Convert.ToInt32(reading.wind_spd_kt);
                BetterList.Add(BetterReading);
            }
            // Database.CheckTableEmpty("Readings");
            DatabaseCon.BuildReadings(BetterList);
            return BetterList;
        }
        public void ListConvert(string inputstring)
        {
            bool found = false;
            if (!(inputstring.Equals("")))
            {
                int WPO = 0;
                string StationName = "";
                for (int i = 0; i < stationlist.Count; i++)
                {
                    if (stationlist[i].Name.Equals(inputstring))
                    {
                        WPO = Convert.ToInt32(stationlist[i].Identifier);
                        StationName = stationlist[i].Name;
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Station does not exist, please check your spelling and try again:");
                    ListConvert(inputstring = Console.ReadLine());
                }
                if (WPO != 0)
                {
                    string Fileident = $"{extractPath}IDQ60910.{WPO}.json";
                    List<RawReadingData> Rawlist = JSONtoOBJ.Objectify(Fileident);
                    StationLocation(Rawlist);
                    List<ReadingDatav2> bindingList = MessyConversion(Rawlist);
                    DatabaseCon.BuildReadings(BetterList);
                    //for (int k = 0; k < 40; k++)
                    //{
                    //    Console.Write("*");
                    //}
                    //Console.WriteLine("");
                    //Console.WriteLine(StationName);
                    //int l = 0;
                    //for (int j = bindingList.Count- 1; j > 0; j--)
                    //{
                        
                    //     Console.WriteLine($"{bindingList[j].ReadingDate}/{bindingList[j].ReadingTime} -- Air Temp: {bindingList[j].ActualTemperature.ToString()},Relative Humidity: {bindingList[j].RelativeHumidity.ToString()},App Temp {bindingList[j].ApparentTemperature.ToString()}");

                    //}
                    //for (int k = 0; k < 20; k++)
                    //{
                    //    Console.Write("*");
                    //}
                    //Console.Read();
                    //Console.Read();
                }
                
            }
        }
        //public void DisplayStations()
        //{
        //    if ((Database.CheckTableEmpty("Stations") == 0) && (Database.CheckTableEmpty("Brisbane") == 0)) 
        //    {
        //        FirstRun();
        //    }
        //    Console.WriteLine("Available weather stations:\n\n");
        //    for (int i = 0; i < stationlist.Count; i++)
        //    {
        //        Console.Write(stationlist[i].Name + "\n\n");
        //    }
        //    ListConvert("Brisbane");
        //}

        public void FileCheck()
        {
            FolderCheck();
            if (IsDirectoryEmpty())
            {
                FTPDownload();
                ExtractTGZ(tempArchiveName, tempArchivePath + "Temp\\");
            }
            string filesCreated = File.GetLastWriteTime(extractPath + "IDQ60910.94170.axf").ToShortDateString();
            string Today = DateTime.Now.ToShortDateString();
            if (filesCreated != Today)
            {
                FTPDownload();
                ExtractTGZ(tempArchiveName, tempArchivePath + "Temp\\");
                Folderactions();
                string yesterday = (DateTime.Now.DayOfWeek - 1).ToString();
                File.Delete(tempArchivePath + "temp" + yesterday + ".tgz");

            }
            else
            {
                Folderactions();
            }
        }
        public void FirstRun()
        {
             DatabaseCon.BuildStations(stationlist);
             
        }
    }
}

