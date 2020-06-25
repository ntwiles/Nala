using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;

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
            Lexer lexer = new Lexer();
            Parser parser = new Parser();
            NalaResult result = new NalaResult();

            List<NalaToken> tokens = null;
            List<ParseNode> parseNodes = null;

            bool parsingSucceeded = false;

            List<string> lines = code.Content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList<string>();

            tokens = lexer.ProcessCode(lines);

            if (tokens != null)
            {
                result.LexingSuccessful = true;
                result.ParsingSuccessful = parser.ProcessTokens(tokens, out parseNodes);
            }
            
            return result;
        }
    }
}
