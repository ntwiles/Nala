using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NathanWiles.Nala;

using NalaWeb.Models;

namespace NalaWeb.Controllers
{
    [Route("api")]
    [ApiController]
    public class NalaController : ControllerBase
    {
        [HttpPost("nala")]
        public ActionResult<NalaResult> RunNala(NalaCode code)
        {
            var webIO = new WebIOContext();
            var result = new NalaResult();

            List<string> lines = code.Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList<string>();

            Nala nala = new Nala(lines, webIO);
            nala.Run();

            result.Output = webIO.Output;
            
            return Ok(result);
        }

        [HttpPost("input")]
        public ActionResult<NalaResult> SubmitInput(NalaInput input)
        {
            var result = new NalaResult();

            //webIO.SendInput(input.Content);
            //result.Output = webIO.Output;

            return Ok(result);
        }
    }
}
