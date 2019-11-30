using InfluxNetCore.Clients;
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
    }
}
