using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Date_Extractor.Services
{
    internal static class BankHolidayRepo
    {

        public static List<Event> GetBankHolidaysEngland()
        {
            // To Do build your list of events from the api

            return new List<Event>();

        }

        //private static string CleanResponse(string response)
        //{

        //}

        public static void SaveDays(List<Day> days, string filename)
        {
            using (TextWriter tw = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["OutputDirectory"], filename)))
            {
                foreach (var day in days)
                {
                    tw.WriteLine($"Date: {day.Date} \t ID: {day.TypeID}");
                }
            }
        }
    }
}
