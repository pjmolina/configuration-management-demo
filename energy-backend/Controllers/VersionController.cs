using Microsoft.AspNetCore.Mvc;

namespace Acme.Energy.Backend.Controllers
{
    [ApiController]
    [Route("version")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public VersionResponseDto Version()
        {
            return new VersionResponseDto("energy-backend", VersionInfo.SemverVersion);
        }
    }

    public class VersionResponseDto
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Ts { get; private set; }
        public VersionResponseDto(string name, string version)
        {
            Name = name;
            Version = version;
            Ts = DateTime.UtcNow.ToString("O");
        }
    }
}
