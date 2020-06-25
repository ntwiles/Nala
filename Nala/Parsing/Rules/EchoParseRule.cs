using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class EchoParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence, IIOContext ioContext)
        {
            for (int i = 0; i < sentence.Count; i++)
            {
                var token = sentence[i];
                if (token.value == "echo" || token.value == "echoline")
                {
                    return IsProper(sentence, ioContext);
                }
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence, IIOContext ioContext)
        {
            if (sentence[0].value != "echo" && sentence[0].value != "echoline")
            {
                new ParseError(this, sentence[0], "Echo calls must begin with either \"echo\" or \"echoline\".").Report(ioContext);
                return false;
            }

            if (sentence[sentence.Count - 1].value != ";")
            {
                new ParseError(this, sentence[sentence.Count - 1], "Echo calls must end with a ';' character.").Report(ioContext);
                return false;
            }

            var expression = sentence.GetRange(1, sentence.Count - 2);
            return new ExpressParseRule().Matches(expression, ioContext);
        }
    }
}
