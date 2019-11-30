using InfluxNetCore.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InfluxNetCore.Clients
{
    internal class PingClient
    {
        public static async Task MakePingRequest()
        {
            await InfluxRequest.MakeGetAsync("ping");
        }

    }
}
