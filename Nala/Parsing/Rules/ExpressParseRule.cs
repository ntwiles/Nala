using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Errors;
using NathanWiles.Nala.IO;
using NathanWiles.Nala.Lexing;

namespace NathanWiles.Nala.Parsing.Rules
{
    public class ExpressParseRule : ParseRule
    {
        public override bool Matches(List<NalaToken> expression, IIOContext ioContext)
        {
            return IsProper(expression, ioContext);
        }

        public override bool IsProper(List<NalaToken> expression, IIOContext ioContext)
        {
            if (expression.Count == 0)
            {
                new ParseError(this, null, "Expression must have at least one operand.").Report(ioContext);
                return false;
            }

            int position = 0;
            int operandLength;

            while (position < expression.Count)
            {
                var operand = checkNextOperand(position, expression, out operandLength, ioContext);

                position += operandLength;

                //We've hit the end and we're still valid. 
                if (position >= expression.Count - 1) { return true; }

                //The next token should be an operator.
                if (!TokenLookups.Operators.Contains(expression[position].value))
                {
                    new ParseError(this, expression[position], "Expected arithmatic operator but got " + expression[position].value).Report(ioContext);
                    return false;
                }
                else
                {
                    position++;
                }

            }

            return true;
        }

        private NalaToken checkNextOperand(int position, List<NalaToken> expression, out int operandLength, IIOContext ioContext)
        {
            NalaToken operand = expression[position];
            operandLength = 1;

            if (operand.type != TokenType.Identifier
                && operand.type != TokenType.IntLiteral
                && operand.type != TokenType.StringLiteral
                && operand.type != TokenType.BoolLiteral)
            {
                new ParseError(this, expression[position], "Token of type \"" + operand.type + "\" does not represent a value.").Report(ioContext);
                return null;
            }

            // We've hit the end of the expression and we're still valid.
            if (position >= expression.Count - 1) { return operand; }

            // We have an indexer.
            if (expression[position + 1].value == "[")
            {
                operandLength = 4;

                // The indexer needs to be an int variable or int literal.
                if (expression[position + 2].type != TokenType.Identifier
                    && expression[position + 2].type != TokenType.IntLiteral)
                {
                    new ParseError(this, expression[position + 2], "Indexer of type \"" + expression[position + 2].type + "\" does not represent a value.").Report(ioContext);
                    return null;
                }

                // We need to make sure the bracket is closed.
                if (expression[position + 3].value != "]") { new ParseError(this, expression[position + 3], "Expected closing bracket.").Report(ioContext); return null; }
            }

            // We have a function invocation.
            if (expression[position + 1].value == "(")
            {
                // We need to make sure the bracket is closed.
                if (expression[position + 3].value != ")") { new ParseError(this, expression[position + 3], "Expected ')'.").Report(ioContext); return null; }

                bool findingParenClose = true;

                while (findingParenClose)
                {

                }

                operandLength = 4;


            }

            return operand;
        }
    }
}
