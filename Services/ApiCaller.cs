using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Date_Extractor.Services
{
    internal static class ApiCaller
    {
        public static async Task<BankHoliday> GetBankHolidaysAsync(string url)
        {
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<BankHoliday>();
            }
            else
            {
                return null;
            }
        }
    }
}
