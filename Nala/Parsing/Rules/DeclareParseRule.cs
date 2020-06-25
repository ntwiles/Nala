using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class DeclareParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence, IIOContext ioContext)
        {
            if (sentence[0].type == TokenType.Primitive) return IsProper(sentence, ioContext);
            else return false;
        }

        public override bool IsProper(List<NalaToken> sentence, IIOContext ioContext)
        {
            //A declaration should always begin with a type.
            if (sentence[0].type != TokenType.Primitive) { new ParseError(this, sentence[0], "Expected type.").Report(ioContext); return false; }

            //Handle normal declaration.
            if (sentence.Count == 3)
            {
                //The 2nd of three elements in a normal declaration should be an identifier. 
                if (sentence[1].type != TokenType.Identifier) { new ParseError(this, sentence[1], "Expected identifier.").Report(ioContext); return false; }
            }

            //Handle array declaration.
            else if (sentence.Count == 6)
            {
                if (sentence[1].value != "[") { new ParseError(this, sentence[1], "Expected opening bracket.").Report(ioContext); return false; }
                if (sentence[2].type != TokenType.Identifier
                    && sentence[2].type != TokenType.IntLiteral)
                {
                    new ParseError(this, sentence[2], "Token of type \"" + sentence[2].type + "\" does not represent a value.");
                    return false;
                }
                if (sentence[3].value != "]") { new ParseError(this, sentence[3], "Expected closing bracket.").Report(ioContext); return false; }
                if (sentence[4].type != TokenType.Identifier) { new ParseError(this, sentence[4], "Expected identifier.").Report(ioContext); return false; }
            }

            // Declarations can only be 3 or 6 tokesn in length;
            else { new ParseError(this, sentence[0], "Wrong number of tokens.").Report(ioContext); return false; }

            //A declaration should always end with a ";".
            if (sentence[sentence.Count - 1].value != ";") { new ParseError(this, sentence[sentence.Count - 1], "Expected ';'.").Report(ioContext); return false; }
            return true;
        }
    }
}
