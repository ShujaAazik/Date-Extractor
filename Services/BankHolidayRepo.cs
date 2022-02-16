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

        public static void SaveDays(List<Day> days, string filename)
        {
            using (TextWriter writer = new StreamWriter(Path.Combine(ConfigurationManager.AppSettings["OutputDirectory"], filename)))
            {
                foreach (var day in days)
                {
                    writer.WriteLine($"Date: {day.Date.ToShortDateString()} \t ID: {day.TypeID}");
                }
            }
        }
    }
}
