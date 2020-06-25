using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.Errors
{
    public class RuntimeError : ErrorReporter
    {
        private string _message;

        public RuntimeError(string message)
        {
            _message = message;
        }

        public void Report()
        {
            Console.WriteLine("Nala Runtime Error: " + _message);
        }
    }



}
