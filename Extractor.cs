using Date_Extractor.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Date_Extractor
{
    internal class Extractor
    {
        private enum DayId { Holiday = 1, Saturday = 2, WorkDay = 3, Sunday = 4 }

        private static DateTime startDate { get; set; }

        private static DateTime endDate { get; set; }


        public static async Task Extract(string dateArgsOne, string dateArgsTwo)
        {
            try
            {
                var dates = DateValidation(dateArgsOne, dateArgsTwo);

                SetStartDate(dates[0]);
                SetEndDate(dates[1]);

                var dateList = GenerateListOfDateInBetween(startDate, endDate);

                var bankHolidayList = await ExtractListOfBankHolidays(ConfigurationManager.AppSettings["Holidays Api"]);

                var dayList = GenerateListOfDays(bankHolidayList, dateList);

                WriteToTextFile(dayList);
            }
            catch (Exception ex)
            {
                File.WriteAllText(Path.Combine(ConfigurationManager.AppSettings["OutputDirectory"], $"Error File"),$"Error \t {ex.Message} found at {DateTime.Now}");
            }
        }

        private static void SetStartDate(DateTime date)
        {
            startDate = date;
        }

        private static void SetEndDate(DateTime date)
        {
            endDate = date;
        }

        private static DateTime[] DateValidation(string dateArgsOne, string dateArgsTwo)
        {
            if (!DateTime.TryParse(dateArgsOne, out DateTime startDate) & !DateTime.TryParse(dateArgsTwo, out DateTime endDate))
            {
                throw new Exception("The Date is invalid. Either of the Date entered is invalid");
            }

            if (startDate.Equals(endDate))
            {
                throw new Exception("Both dates cannot be equal.");
            }
            else if (startDate > endDate)
            {
                throw new Exception("The end date should be a future date than the start date");
            }

            return new DateTime[] { startDate, endDate };
        }

        private static List<DateTime> GenerateListOfDateInBetween(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(0, endDate.Subtract(startDate).Days + 1).Select(x => startDate.AddDays(x)).ToList();
        }

        private static async Task<List<DateTime>> ExtractListOfBankHolidays(string bankHolidayApi)
        {
            var holidays = await ApiCaller.GetBankHolidaysAsync(bankHolidayApi);

            var bankHolidayList = new List<DateTime>();

            foreach (var division in new List<Division> { holidays.EnglandAndWales, holidays.Scotland, holidays.NorthernIreland })
            {
                if (!(division is null))
                {
                    bankHolidayList.AddRange(division.events.Select(offset => offset.Date).ToList());
                }
            }

            return bankHolidayList;
        }

        private static List<Day> GenerateListOfDays(List<DateTime> bankHolidayList, List<DateTime> dateList)
        {
            var dayList = new List<Day>();

            var weekDays = new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            foreach (var date in dateList)
            {
                if (bankHolidayList.Any(holdayDate => holdayDate.Equals(date)))
                {
                    dayList.Add(new Day { Date = date, TypeID = (int)DayId.Holiday });
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    dayList.Add(new Day { Date = date, TypeID = (int)DayId.Saturday });
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

            return dayList;
        }

        private static void WriteToTextFile(List<Day> dayList)
        {
            BankHolidayRepo.SaveDays(dayList, $"From {startDate.ToString("dd MMMM yyyy")} to {endDate.ToString("dd MMMM yyyy")}");
        }

    }
}
