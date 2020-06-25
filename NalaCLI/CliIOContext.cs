using NathanWiles.Nala.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace NalaCLI
{
    class CliIOContext : IIOContext
    {
        public void Clear()
        {
            Console.Clear();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
