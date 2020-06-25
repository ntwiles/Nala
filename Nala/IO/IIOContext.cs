using System;
using System.Collections.Generic;
using System.Text;

namespace NathanWiles.Nala.IO
{
    public interface IIOContext
    {
        void Clear();
        string ReadLine();
        void Write(string message);
        void WriteLine(string message);
    }
}
