using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Date_Extractor
{
    internal class Program
    {
        enum DayId { Holiday = 1, Saturday = 2, WorkDay = 3, Sunday = 4 }

        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;

            //var httpResponse = WebRequest.Create(ConfigurationManager.AppSettings[""Holidays Api""]).GetResponse();

            var url = ConfigurationManager.AppSettings["Holidays Api"];

            string json = @"{""england - and - wales"":{""division"":""england - and - wales"",""events"":[{""title"":""New Year’s Day"",""date"":""2017 - 01 - 02"",""notes"":""Substitute day"",""bunting"":true},{""title"":""Good Friday"",""date"":""2017 - 04 - 14"",""notes"":"""",""bunting"":false}]}}";

            var holidays = JsonSerializer.Deserialize<Division>(json);

            Console.WriteLine("Enter a Valid Date:");

            //var date = Console.ReadLine();

            //File.WriteAllLines("Path",new string[6]);
        }
    }
}
