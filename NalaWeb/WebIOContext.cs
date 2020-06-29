using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NathanWiles.Nala.IO;

namespace NalaWeb
{
    public class WebIOContext : IIOContext
    {
        private bool waitingForInput;
        public List<string> Output { get; }
        private string Input;

        public WebIOContext()
        {
            Output = new List<string>();
        }

        public void Clear()
        {
            Output.Clear();
        }

        public string ReadLine()
        {
            waitingForInput = true;

            while (waitingForInput) { }

            return Input;
        }

        public void Write(string message)
        {
            Output.Add(message);
        }

        public void WriteLine(string message)
        {
            Output.Add(message + "<br>");
        }

        public void SendInput(string input)
        {
            Input = input;
            waitingForInput = false;
        }
    }
}
