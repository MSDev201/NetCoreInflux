using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxNetCore.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static void AddInfluxClient(this IServiceCollection service, string host = "localhost", ushort port = 8086, string username = null, string password = null)
        {
            // register as a service
            service.AddSingleton<IInfluxClient, InfluxClient>();
        }

    }
}
