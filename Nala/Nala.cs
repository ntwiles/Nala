using System;
using System.Collections.Generic;
using System.Text;

using NathanWiles.Nala.Lexing;
using NathanWiles.Nala.Parsing;
using NathanWiles.Nala.Interpreting;

namespace NathanWiles.Nala
{
    public class Nala
    {
        private List<string> codeLines;

        public Nala(List<string> codeLines)
        {
            this.codeLines = codeLines;
        }

        public bool Run()
        {
            var lexer = new Lexer();
            var parser = new Parser();
            var interpreter = new Interpreter();

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
                Console.WriteLine("Lexer exception:");
                Console.WriteLine(e.Message);
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
                Console.WriteLine("Parser exception:");
                Console.WriteLine(e.Message);
                return false;
            }

            // Execute parse tree.
            try
            {
                return interpreter.Execute(nalaParseTree, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Interpreter exception:");
                Console.WriteLine(e.Message);
            }

            return false;
        }

    }
}
