using Microsoft.AspNetCore.Mvc;

namespace Acme.Energy.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        public ActionResult<PingResponse> Ping()
        {
            return new PingResponse("pong");
        }
    }

    public class PingResponse
    {
        public PingResponse(string message)
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
