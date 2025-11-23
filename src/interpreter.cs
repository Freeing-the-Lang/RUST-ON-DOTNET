// File: interpreter.cs

using System;
using System.Collections.Generic;

namespace RustOnDotnet
{
    public class Interpreter
    {
        private readonly Dictionary<string, int> Env = new();

        public int Eval(Node n)
        {
            return n switch
            {
                NumberLit num => num.Value,

                IdentExpr id =>
                    Env.ContainsKey(id.Name)
                        ? Env[id.Name]
                        : throw new Exception($"Unknown variable: {id.Name}"),

                LetDecl let =>
                (
                    Env[let.Name] = Eval(let.Value)
                ),

                Binary bin when bin.Op == "+" =>
                    Eval(bin.Left) + Eval(bin.Right),

                _ => throw new Exception("Unsupported node")
            };
        }
    }
}
