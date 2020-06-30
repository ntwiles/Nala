using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NathanWiles.Nala;

using NalaWeb.Models;
using NalaWeb.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace NalaWeb.Controllers
{ 
    [Route("api")]
    [ApiController]
    public class NalaController : ControllerBase
    {
        private readonly IHubContext<NalaHub> hubContext;

        public NalaController (IHubContext<NalaHub> nalaHub)
        {
            this.hubContext = nalaHub;
        }

        [HttpPost("nala")]
        public ActionResult UpdateOutputAsync(NalaCode code)
        {
            var webIO = new WebIOContext(hubContext);

            List<string> lines = code.Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList<string>();

            Nala nala = new Nala(lines, webIO);
            nala.Run();

            return Ok();
        }
    }
}
