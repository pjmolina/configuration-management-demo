using Microsoft.AspNetCore.Mvc;
using Consul;
using System.Text;

namespace Acme.Energy.Backend.Controllers
{
    [ApiController]
    [Route("/consul-read")]
    public class ConsulReadController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ConsulReadController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse>> ConsulRead()
        {
            var consulHost = _config.GetValue<string>("Consul:Host");
            var consulPort = _config.GetValue<string>("Consul:Port");

            Console.WriteLine("Conecting to consul at: " + consulHost + ":" + consulPort);

            using (var client = new ConsulClient(x =>
                    x.Address = new Uri($"http://{consulHost}:{consulPort}"))
                )
            {
                return new DataResponse
                {
                    Secret = await ReadKey(client, "acme/energy/shared-secret1"),
                    Port = await ReadKey(client, "acme/energy/port"),
                    EnabledFeatureA = await ReadKey(client, "acme/energy/features/a") == "true",
                    EnabledFeatureB = await ReadKey(client, "acme/energy/features/b") == "true",
                    EnabledFeatureC = await ReadKey(client, "acme/energy/features/c") == "true",
                };
            }
        }
        private async Task<string> ReadKey(ConsulClient client, string key)
        {
            try
            {
                var getPair = await client.KV.Get(key);
                return Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
            }
            catch 
            {
                return "";
            }
        }
 
    }

    public class DataResponse
    {
        public string Secret { get; set; } = "";
        public string Port { get; set; } = "";
        public bool EnabledFeatureA { get; set; } 
        public bool EnabledFeatureB { get; set; }
        public bool EnabledFeatureC { get; set; }
    }
}
