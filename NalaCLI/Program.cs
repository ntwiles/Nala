
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NalaCLI;

namespace NathanWiles.Nala
{
    class Program
    {
        private static bool argShowCode = false, argDebug = false;
        private static string argFileName = "";

        static void Main(string[] args)
        {
            // Don't run the program if we were fed invalid command-line arguments.     
            if (!HandleArgs(args)) { return; }

            string fullPath = getSourceCodePath(argFileName);

            List<string> nalaCodeLines = loadNalaFile(fullPath);

            // Write nala code to console if the flag was set.
            if (argShowCode)
            {
                writeOutCodeLines(nalaCodeLines);
            }
            else
            {
                Console.Clear();
            }

            // Run nala code.
            var nala = new Nala(nalaCodeLines, new CliIOContext());
            bool success = nala.Run();
            if (!success) Console.ReadKey();
        }

        static void writeOutCodeLines(List<string> nalaCodeLines)
        {
            foreach (string line in nalaCodeLines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine("");
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

        static bool HandleArgs(string[] args)
        {
            // Explain nala command syntax.
            if (args.Length < 1)
            {
                Console.WriteLine("\nLoad a nala (.nl) file by supplying its path as the first argument.");
                Console.WriteLine("e.g.");
                Console.WriteLine("nala main.nl");
                Console.WriteLine("\nUse the -show flag to output the content of the given .nl file.");
                return false;
            }
            else
            {
                // Read command flags.
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i];

                    switch (arg)
                    {
                        case "-show": argShowCode = true; break;
                        case "-s": argShowCode = true; break;
                        case "-debug": argDebug = true; break;
                        case "-d": argDebug = true; break;
                        default: Console.WriteLine("Unknown flag."); return false;
                    }
                }

                // Load .nl file.
                argFileName = args[0];

                return true;
            }
        }

        private static string getSourceCodePath(string fileName)
        {
            if (!fileName.EndsWith(".nl") && !fileName.EndsWith(".nl"))
            {
                fileName += ".nl";
            }

            var path = Environment.CurrentDirectory;

            return Path.Combine(path, fileName);
        }
    }
}
