using Microsoft.AspNetCore.Mvc;

namespace Acme.Energy.Backend.Controllers
{
    [ApiController]
    [Route("/server-config")]
    public class ServerConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ServerConfigController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public ActionResult<IDictionary<string, string>> ServerConfiguration()
        {
            var dic = new Dictionary<string, string>
            {
                { "Environment", _config.GetValue<string>("ENV") },
                { "Hostname", System.Net.Dns.GetHostName() },
                { "Port", _config.GetValue<string>("PORT") },
                { "Version", VersionInfo.SemverVersion },
                { "ASPNETCORE_URLS", _config.GetValue<string>("ASPNETCORE_URLS")},
                { "Consul.Hostname", _config.GetValue<string>("CONSUL:HOST")},
                { "Consul.Port", _config.GetValue<string>("CONSUL:PORT")}
            };

            return Ok(dic);  
        }
    }

}
