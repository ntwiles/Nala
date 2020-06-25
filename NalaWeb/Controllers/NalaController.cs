using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.IO;
using NathanWiles.Nala;

using NalaWeb.Models;

namespace NalaWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NalaController : ControllerBase
    {

        [HttpGet]
        public NalaCode Index()
        {
            NalaCode poco = new NalaCode();
            poco.Content = "This is the message at the index.";
            return poco;
        }

        [HttpPost]
        public NalaResult TestNala(NalaCode code)
        {
            WebIOContext webIO = new WebIOContext();
            NalaResult result = new NalaResult();

            List<string> lines = code.Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList<string>();

            /*
            Lexer lexer = new Lexer(webIO);
            Parser parser = new Parser(webIO);

            List<NalaToken> tokens = null;
            List<ParseNode> parseNodes = null;



            result.LexingSuccessful = lexer.TryProcessCode(lines, out tokens);


            if (result.LexingSuccessful)
            {
                result.ParsingSuccessful = parser.TryProcessTokens(tokens, out parseNodes);
            }
            */

            Nala nala = new Nala(lines, webIO);
            nala.Run();

            result.Output = webIO.Output;
            
            return result;
        }
    }
}
