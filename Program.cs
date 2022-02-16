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

        static async Task Main(string[] args)
        {
            await Extractor.Extract(args[0], args[1]); 
        }
    }
}
