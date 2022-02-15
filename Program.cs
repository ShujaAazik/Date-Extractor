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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Date_Extractor.Services;

namespace Date_Extractor
{
    internal class Program
    {
        enum DayId { Holiday = 1, Saturday = 2, WorkDay = 3, Sunday = 4 }
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            DateTime.TryParse(args[0], out DateTime startDate);
            DateTime.TryParse(args[1], out DateTime endDate);
            var dateList = Enumerable.Range(0, endDate.Subtract(startDate).Days + 1).Select(x => startDate.AddDays(x)).ToList();
            var dayList = new List<Day>();

            var holidays = await ApiCaller.GetBankHolidaysAsync(ConfigurationManager.AppSettings["Holidays Api"]);

            var bankHolidayList = new List<DateTime>();

            foreach (var division in new List<Division> { holidays.EnglandAndWales, holidays.Scotland, holidays.NorthernIreland })
            {
                if (!(division is null))
                {
                    bankHolidayList.AddRange(division.events.Select(offset => offset.Date).ToList());
                }
            }

            var weekDays = new DayOfWeek[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday};

            foreach (var date in dateList)
            {
                if (bankHolidayList.Any(holdayDate => holdayDate.Equals(date)))
                {
                    dayList.Add(new Day { Date = date, TypeID = (int) DayId.Holiday}) ;
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    dayList.Add(new Day { Date = date, TypeID = (int) DayId.Saturday});
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dayList.Add(new Day { Date = date, TypeID = (int)DayId.Sunday });
                }
                else if (weekDays.Any(day => day.Equals(date.DayOfWeek)))
                {
                    dayList.Add(new Day { Date = date, TypeID = (int)DayId.WorkDay });
                }
            }

            BankHolidayRepo.SaveDays(dayList, $"From {startDate.ToString("dd MMMM yyyy")} to {endDate.ToString("dd MMMM yyyy")}");
        }
    }
}
