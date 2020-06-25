using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Errors;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class EchoParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence)
        {
            for (int i = 0; i < sentence.Count; i++)
            {
                var token = sentence[i];
                if (token.value == "echo" || token.value == "echoline")
                {
                    return IsProper(sentence);
                }
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence)
        {
            if (sentence[0].value != "echo" && sentence[0].value != "echoline")
            {
                new ParseError(this, sentence[0], "Echo calls must begin with either \"echo\" or \"echoline\".").Report();
                return false;
            }

            if (sentence[sentence.Count - 1].value != ";")
            {
                new ParseError(this, sentence[sentence.Count - 1], "Echo calls must end with a ';' character.").Report();
                return false;
            }

            var expression = sentence.GetRange(1, sentence.Count - 2);
            return new ExpressParseRule().Matches(expression);
        }
    }
}
