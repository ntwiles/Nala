using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.NalaConsole
{
    public class NalaConsoleViewModel : TextWriter
    {
        private NalaConsoleModel _console;

        public NalaConsoleViewModel()
        {
            _console = new NalaConsoleModel();
        }

        public override void Write(char value)
        {
            _console.Output.Add(""+value);
        }

        public override void Write(string value)
        {
            _console.Output.Add(value);
        }

        public override Encoding Encoding
        {
            get
            {
                return Encoding.ASCII;
            }
        }
    }
}
