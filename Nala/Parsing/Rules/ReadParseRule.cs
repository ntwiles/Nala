using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class ReadParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence, IIOContext ioContext)
        {
            for (int i = 0; i < sentence.Count; i++)
            {
                var token = sentence[i];
                if (token.value == "read")
                {
                    return IsProper(sentence, ioContext);
                }
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence, IIOContext ioContext)
        {
            bool hasIdentifier = false;

            // First token should be 'read';
            if (sentence[0].value != "read")
            {
                new ParseError(this, sentence[0], "Read calls must begin with \"read\".").Report(ioContext);
                return false;
            }

            // Middle token, if we have one, should be an identifier.
            if (sentence.Count == 3)
            {
                if (sentence[1].type != TokenType.Identifier)
                {
                    new ParseError(this, sentence[1], "Read call values must be assigned to an identifier.").Report(ioContext);
                    return false;
                }
                else hasIdentifier = true;
            }

            // Last token should be a ";"
            if (sentence[sentence.Count - 1].value != ";")
            {
                new ParseError(this, sentence[sentence.Count - 1], "Read calls must end with a ';' character.").Report(ioContext);
                return false;
            }

            if (hasIdentifier)
            {
                var expression = sentence.GetRange(1, sentence.Count - 2);
                return new ExpressParseRule().Matches(expression, ioContext);
            }

            return true;
        }
    }
}
