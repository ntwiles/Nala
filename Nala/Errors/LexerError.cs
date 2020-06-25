using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.Errors
{
    class LexerError : ErrorReporter
    {
        private int _line, _column;
        private string _message;

        public LexerError(int line, int column, string message)
        {
            _line = line;
            _column = column;
            _message = message;
        }
        public void Report()
        {
            Console.WriteLine("Nala Lexer Error: Line " + _line + " column " + _column + " : " + _message);
        }
    }
}
