using InfluxNetCore.Clients;
using InfluxNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfluxNetCore
{
    public class InfluxClient : IInfluxClient
    {
        internal static InfluxConnection Connection { get; set; }


        public async Task Test()
        {
            new Task(() =>
            {
                while(true)
                {
                    _ = WriteClient.MakeWriteRequest("testMeasure", new
                    {
                        Zufallszahl = new Random().Next(0, 10000),
                        Zufalldouble = new Random().NextDouble() * 10000d,
                    }, new
                    {
                        Valide = true,
                        Invalide = false,
                        Yesterday = DateTime.Now.AddDays(-1)
                    });
                    Thread.Sleep(50);
                }
            }).Start();

            
        }

        public async Task WritePoint(
            string measurement,
            object fields,
            object tags = null,
            DateTime? time = null,
            string precision = null,
            string db = null)
        {
            string timestamp = null;
            if (time.HasValue)
                timestamp = time.Value.GetUnixTimestampNanoseconds().ToString();

            await WriteClient.MakeWriteRequest(measurement, fields, tags, timestamp, precision, db);
        }
    }
}
