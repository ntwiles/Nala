
using System;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing.Rules;
using NathanWiles.Nala.Interpreting;
using System.Runtime;
using NathanWiles.Nala.IO;

namespace NathanWiles.Nala.Errors
{
    public class ParseError : ErrorReporter
    {
        private NalaToken _token;
        private string _message, _errorOut;

        public ParseError(NalaToken token, string message)
        {
            _token = token;
            _message = message;

            _errorOut = "Nala Parse Error: Line " + (_token.line + 1) + " column " + _token.column + ": " + _message;
        }

        public ParseError(ParseRule rule, NalaToken token, string message)
        {
            string parseType = "";

            // TODO This is not polymorphic. ParseType should come from the parseRule.
            switch (rule)
            {
                case DeclareParseRule decl: parseType = "declaration"; break;
                case AssignParseRule assign: parseType = "assignment"; break;
                case EchoParseRule echo: parseType = "echo call"; break;
                case ReadParseRule read: parseType = "read call"; break;
                case ClearParseRule clear: parseType = "clear call"; break;
                case ExpressParseRule express: parseType = "expression"; break;
                case ParamsParseRule param: parseType = "parameter"; break;
            }

            _errorOut = "Nala Parse Rule Error: Malformed " + parseType + " at line " + (token.line + 1) + " column " + token.column + ": " + message;
        }

        public void Report(IIOContext ioContext)
        {
            ioContext.WriteLine(_errorOut);
        }
    }
}
