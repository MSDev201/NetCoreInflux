using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxNetCore.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static void AddInfluxClient(this IServiceCollection service, string host = "localhost", ushort port = 8086, string username = null, string password = null, string defaultDb = null)
        {
            // register connection
            var connection = new InfluxConnection
            {
                Host = host,
                Port = port,
                Username = username,
                Password = password,
                DefaultDatabase = defaultDb
            };

            InfluxClient.Connection = connection;

            // register as a service
            service.AddSingleton<IInfluxClient, InfluxClient>();
        }

        public static void AddInfluxClient(this IServiceCollection services)
        {
            var serviceProv = services.BuildServiceProvider();
            var config = serviceProv.GetService<IConfiguration>();
            var section = config.GetSection("InfluxClient");

            if (!section.Exists())
                return;

            var sectionChilds = section.GetChildren();
            string host = null;
            ushort port = 0;
            string username = null, password = null, defaultDb = null;
            foreach(var sectionChild in sectionChilds)
            {
                switch (sectionChild.Key.ToLower())
                {
                    case "host":
                        host = sectionChild.Value;
                        break;
                    case "port":
                        ushort.TryParse(sectionChild.Value, out port);
                        break;
                    case "username":
                        username = sectionChild.Value;
                        break;
                    case "password":
                        password = sectionChild.Value;
                        break;
                    case "database":
                        defaultDb = sectionChild.Value;
                        break;
                    default:
                        break;
                }

            }

            AddInfluxClient(services, host, port, username, password, defaultDb);
        }

    }
}
