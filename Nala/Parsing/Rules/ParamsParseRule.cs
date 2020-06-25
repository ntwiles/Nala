using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    // Parameters inside function declaration.
    public class ParamsParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> expression)
        {
            return IsProper(expression);
        }

        public override bool IsProper(List<NalaToken> expression)
        {
            int position = 1;

            while (position < expression.Count - 2)
            {
                if (expression[position].type != TokenType.Primitive) { new ParseError(this, expression[position], $"Expected type but got '{expression[position].value}'").Report(); return false; }
                if (expression[position + 1].type != TokenType.Identifier) { new ParseError(this, expression[position], $"Expected identifier but got '{expression[position + 1]}'.").Report(); return false; }
                if (expression[position + 2].value == ",")
                {
                    position += 3;
                }
                else { break; }
            }

            return true;
        }
    }
}
