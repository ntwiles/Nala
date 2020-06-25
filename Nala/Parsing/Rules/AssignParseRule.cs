using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class AssignParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence, IIOContext ioContext)
        {
            for (int i = 0; i < sentence.Count; i++)
            {
                // Look for one of the following operators: = += -= *= /=
                if (TokenLookups.AssignOperators.Contains(sentence[i].value))
                {
                    return IsProper(sentence, ioContext);
                }
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence, IIOContext ioContext)
        {
            // An assignment should always begin with an identifier token.
            if (sentence[0].type != TokenType.Identifier) { new ParseError(this, sentence[0], "Expected identifier.").Report(ioContext); return false; }

            List<NalaToken> expression = null;

            // Normal assignments will have the assignment operator at the second position.
            if (TokenLookups.AssignOperators.Contains(sentence[1].value))
            {
                expression = sentence.GetRange(2, sentence.Count - 3);
            }

            // Assignments to array indices will have the assignment operator at the 5th position.
            else if (TokenLookups.AssignOperators.Contains(sentence[4].value))
            {
                expression = sentence.GetRange(5, sentence.Count - 6);

                // A "[" should follow the first identifier.
                if (sentence[1].value != "[") { new ParseError(this, sentence[1], "Expected opening bracket.").Report(ioContext); return false; }

                // Inside the two brackets should be an identifier or an integer literal.
                if (sentence[2].type != TokenType.Identifier
                    && sentence[2].type != TokenType.IntLiteral)
                {
                    new ParseError(this, sentence[2], "Token of type \"" + sentence[2].type + "\" does not represent an integer value.").Report(ioContext);
                    return false;
                }

                // The closing "]".
                if (sentence[3].value != "]") { new ParseError(this, sentence[1], "Expected closing bracket.").Report(ioContext); return false; }
            }

            // If the assignment operator isn't in one of those two positions, the sentence is malformed.
            else
            {
                new ParseError(this, sentence[1], "Expected assignment operator.").Report(ioContext);
                return false;
            }

            // Every assignment should end with a ";".
            if (sentence[sentence.Count - 1].value != ";")
            {
                new ParseError(this, sentence[sentence.Count - 1], "Assignment operations must end with a ';' character.").Report(ioContext);
                return false;
            }

            return new ExpressParseRule().Matches(expression, ioContext);
        }
    }
}
