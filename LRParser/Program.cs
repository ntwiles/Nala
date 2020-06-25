using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using NathanWiles.Nala.Lexing;

namespace LRParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexer lexer = new Lexer();

            List<string> nala = loadNalaFile($"{args[0]}.nl");

            foreach (string line in nala)
            {
                Console.WriteLine(line);
            }

            List<NalaToken> tokens = lexer.ProcessCode(nala);

            foreach (NalaToken token in tokens)
            {
                Console.WriteLine($"{token.value} ({token.type})");
            }

            //LRParser parser = new LRParser();

            Console.ReadLine();
        }

        static List<string> loadNalaFile(string fullPath)
        {
            try
            {
                // Read all the lines of code into memory.
                List<string> nalaCode = File.ReadAllLines(fullPath).ToList();
                return nalaCode;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
