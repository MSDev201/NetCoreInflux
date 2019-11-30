using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxNetCore
{
    internal class InfluxConnection
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
