using Consul;

namespace Acme.Energy.Backend
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// AppBuilderExtension to register a service in Consul
        /// </summary>
        public static IApplicationBuilder RegisterInConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, ConsulServiceEntity serviceEntity)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{serviceEntity.ConsulHost}:{serviceEntity.ConsulPort}"));
            var httpCheck = new AgentServiceCheck()
            {
                Name = "ping",
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // How long does it take to register after the service starts
                Interval = TimeSpan.FromSeconds(10), // Health check interval, or heartbeat interval
                HTTP = $"http://{serviceEntity.Host}:{serviceEntity.Port}/ping", // Health check address
                Timeout = TimeSpan.FromSeconds(5)
            };

            var environment = Environment.GetEnvironmentVariable("ENV");
            var version = VersionInfo.SemverVersion.ToString();

            var hostname = System.Net.Dns.GetHostName();
            var ips = System.Net.Dns.GetHostAddresses(hostname);
            var ip = ips != null ? ips[0].ToString() : "0.0.0.0";
            var port = Environment.GetEnvironmentVariable("PORT");

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceEntity.ServiceName,
                Address = serviceEntity.Host,
                Port = serviceEntity.Port,
                Tags = new[] {
                    serviceEntity.ServiceName,
                    "energy-api",
                    "v" + SafeForDns(version),
                    "env-" + environment,
                },
                Meta = new Dictionary<string, string>
                {
                    { "service", serviceEntity.ServiceName },
                    { "env", environment ?? "" },
                    { "version", version },
                    { "hostname", hostname },
                    { "ip", ip },
                    { "port", port ?? "" },
                }
            };

            consulClient.Agent.ServiceRegister(registration).Wait(); // Register when the service starts, the internal implementation is actually to register using the Consul API (initiated by HttpClient)
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();// Unregister when the service stops
            });
            return app;
        }
        private static string SafeForDns(string value) 
        {
            return (value ?? "").Replace(".", "-").Replace(".", "-");
        }
    }
    /// <summary>
    /// Register for Consul Registration
    /// </summary>
    public class ConsulServiceEntity
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 0;
        public string ServiceName { get; set; } = "";
        public string ConsulHost { get; set; } = "";
        public int ConsulPort { get; set; } = 0;
    }

    public static class ConsulRegistrator
    {
        /// <summary>
        /// Microservice registration in Consul Registration
        /// </summary>
        public static void RegisterInConsul(IConfiguration configuration, IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {
            var hostname = System.Net.Dns.GetHostName();

            ConsulServiceEntity serviceEntity = new()
            {
                Host = configuration.GetValue("Ip", hostname),
                Port = configuration.GetValue("PORT", 80),
                ServiceName = configuration.GetValue("Consul:ServiceName", "energy-backend"),
                ConsulHost = configuration.GetValue("Consul:Host", "not-configured"),
                ConsulPort = configuration.GetValue("Consul:Port", 8500)
            };
            if (serviceEntity.ConsulHost != "not-configured")
            {
                Console.WriteLine("Registering in Consul with values:");
                Console.WriteLine($"  - ServiceName: {serviceEntity.ServiceName}");
                Console.WriteLine($"  - Service at:  {serviceEntity.Host}:{serviceEntity.Port}");
                Console.WriteLine($"  - Consul at:   {serviceEntity.ConsulHost}:{serviceEntity.ConsulPort}");

                app.RegisterInConsul(lifetime, serviceEntity);
            }
            else
            {
                Console.WriteLine("Skipped Consul registration.");
            }
        }
    }
}
