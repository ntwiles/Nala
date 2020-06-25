using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NathanWiles.Nala.IO;

namespace NalaWeb
{
    public class WebIOContext : IIOContext
    {
        public List<string> Output { get; }

        public WebIOContext()
        {
            Output = new List<string>();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void Write(string message)
        {
            Output.Add(message);
        }

        public void WriteLine(string message)
        {
            Output.Add(message + "<br>");
        }
    }
}
