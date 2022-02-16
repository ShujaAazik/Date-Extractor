using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Date_Extractor
{
    internal class Event
    {
        public string title { get; set; }

        public DateTime Date { get; set; }

        public string notes { get; set; }

        public bool bunting { get; set; }

        public string date
        {
            set { Date = DateTime.Parse(value); }
        }
    }
}
