// File: main.cs

using System;
using System.Collections.Generic;
using System.IO;

namespace RustOnDotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Rust-on-Dotnet]");
            Console.WriteLine("Prototype runtime starting...\n");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: rustdot <file.rust>");
                return;
            }

            string file = args[0];

            if (!File.Exists(file))
            {
                Console.WriteLine("Source not found: " + file);
                return;
            }

            string src = File.ReadAllText(file);

            Console.WriteLine("Source:");
            Console.WriteLine("-------------------------------");
            Console.WriteLine(src);
            Console.WriteLine("-------------------------------\n");

            // LEX
            Lexer lx = new Lexer(src);
            var tokens = new List<Token>();
            Token tk;
            while ((tk = lx.Next()).Kind != TokenKind.EOF)
                tokens.Add(tk);

            Console.WriteLine("Tokens:");
            foreach (var t in tokens)
                Console.WriteLine(" " + t);

            // PARSE
            Parser p = new Parser(tokens);
            Node ast = p.Parse();

            Console.WriteLine("\nAST parsed.");

            // INTERPRET
            Interpreter vm = new Interpreter();
            int result = vm.Eval(ast);

            Console.WriteLine("\nInterpreter Result = " + result);

            // IL EMIT (Prototype)
            Console.WriteLine("\n>> Running IL prototype...");
            var ilgen = new ILGeneratorPrototype();
            ilgen.EmitHello();

            Console.WriteLine("\n[Done]");
        }
    }
}

