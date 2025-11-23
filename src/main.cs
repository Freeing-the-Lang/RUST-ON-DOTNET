// File: main.cs
// Prototype: Rust-on-Dotnet runtime loader

using System;

namespace RustOnDotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Rust-on-Dotnet]");
            Console.WriteLine("Prototype runtime booting...");

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: rustdot <file.rust>");
                return;
            }

            string file = args[0];

            if (!System.IO.File.Exists(file))
            {
                Console.WriteLine("Source not found: " + file);
                return;
            }

            string src = System.IO.File.ReadAllText(file);
            Console.WriteLine("Loaded Rust file:");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(src);
            Console.WriteLine("--------------------------------");

            // 실제 Rust -> IL 변환 준비 포인트
            Console.WriteLine("Tokenizing Rust code (prototype)...");
            SimpleLexer lx = new SimpleLexer(src);

            Token tk;
            while ((tk = lx.Next()).Kind != TokenKind.EOF)
            {
                Console.WriteLine(tk);
            }

            Console.WriteLine("\nRuntime ready.");
        }
    }
}
