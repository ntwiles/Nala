using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Interpreting;
using NathanWiles.Nala.IO;

namespace NathanWiles.Nala
{
    public class Nala
    {
        private List<string> codeLines;
        private IIOContext ioContext;

        public Nala(List<string> codeLines, IIOContext ioContext)
        {
            this.codeLines = codeLines;
            this.ioContext = ioContext;
        }

        public bool Run()
        {
            var lexer = new Lexer(ioContext);
            var parser = new Parser(ioContext);
            var interpreter = new Interpreter(ioContext);

            List<NalaToken> nalaTokens = null;
            List<ParseNode> nalaParseTree = null;

            bool lexingSucceeded = false;
            bool parsingSucceeded = false;

            // Lex code into tokens.
            try
            {
                lexingSucceeded = lexer.TryProcessCode(codeLines, out nalaTokens);
                if (!lexingSucceeded) { return false; }
            }
            catch (Exception e)
            {
                ioContext.WriteLine("Lexer exception:");
                ioContext.WriteLine(e.Message);
                return false;
            }

            // Parse tokens into parse tree.
            try
            {
                parsingSucceeded = parser.TryProcessTokens(nalaTokens, out nalaParseTree);
                if (!parsingSucceeded) { return false; }
            }
            catch (Exception e)
            {
                ioContext.WriteLine("Parser exception:");
                ioContext.WriteLine(e.Message);
                return false;
            }

            // Execute parse tree.
            try
            {
                return interpreter.Execute(nalaParseTree, null);
            }
            catch (Exception e)
            {
                ioContext.WriteLine("Interpreter exception:");
                ioContext.WriteLine(e.Message);
            }

            return false;
        }

    }
}
