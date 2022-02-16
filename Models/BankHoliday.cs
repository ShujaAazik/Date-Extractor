using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Date_Extractor
{
    internal class BankHoliday
    {
        [JsonProperty("england-and-wales")]
        public Division EnglandAndWales { get; set; }

        [JsonProperty("scotland")]
        public Division Scotland { get; set; }

        [JsonProperty("northern-ireland")]
        public Division NorthernIreland { get; set; }

        //public string country { get; set; }

        //public List<Event> Events { get; set; }
    }
}
