using System;
using System.Collections.Generic;
using System.IO;

namespace RustOnDotnet
{
    class MainApp
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Rust-on-DOTNET v0.1 ===");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: rustdot <file.rust>");
                return;
            }

            string path = args[0];
            string src = File.ReadAllText(path);

            Console.WriteLine("\nSource Code:");
            Console.WriteLine("---------------------------------");
            Console.WriteLine(src);
            Console.WriteLine("---------------------------------\n");

            Lexer lx = new Lexer(src);
            var toks = new List<Token>();
            Token tk;

            while ((tk = lx.Next()).Kind != TokenKind.EOF)
                toks.Add(tk);

            Console.WriteLine("Tokens:");
            foreach (var t in toks)
                Console.WriteLine(" " + t);

            Parser p = new Parser(toks);
            Node ast = p.Parse();

            Console.WriteLine("\nAST Ready.");

            Interpreter vm = new Interpreter();
            int result = vm.Eval(ast);

            Console.WriteLine("\nInterpreter Result = " + result);

            Console.WriteLine("\nRunning IL backend test...");
            new ILEmit().EmitHello();

            Console.WriteLine("\n[Done]");
        }
    }
}

