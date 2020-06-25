using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class AssignParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> sentence)
        {
            for (int i = 0; i < sentence.Count; i++)
            {
                // Look for one of the following operators: = += -= *= /=
                if (TokenLookups.AssignOperators.Contains(sentence[i].value))
                {
                    return IsProper(sentence);
                }
            }

            return false;
        }

        public override bool IsProper(List<NalaToken> sentence)
        {
            // An assignment should always begin with an identifier token.
            if (sentence[0].type != TokenType.Identifier) { new ParseError(this, sentence[0], "Expected identifier.").Report(); return false; }

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
                if (sentence[1].value != "[") { new ParseError(this, sentence[1], "Expected opening bracket.").Report(); return false; }

                // Inside the two brackets should be an identifier or an integer literal.
                if (sentence[2].type != TokenType.Identifier
                    && sentence[2].type != TokenType.IntLiteral)
                {
                    new ParseError(this, sentence[2], "Token of type \"" + sentence[2].type + "\" does not represent an integer value.").Report();
                    return false;
                }

                // The closing "]".
                if (sentence[3].value != "]") { new ParseError(this, sentence[1], "Expected closing bracket.").Report(); return false; }
            }

            // If the assignment operator isn't in one of those two positions, the sentence is malformed.
            else
            {
                new ParseError(this, sentence[1], "Expected assignment operator.").Report();
                return false;
            }

            // Every assignment should end with a ";".
            if (sentence[sentence.Count - 1].value != ";")
            {
                new ParseError(this, sentence[sentence.Count - 1], "Assignment operations must end with a ';' character.").Report();
                return false;
            }

            return new ExpressParseRule().Matches(expression);
        }
    }
}
