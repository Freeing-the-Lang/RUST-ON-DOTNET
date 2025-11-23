using System;
using System.Collections.Generic;

namespace RustOnDotnet
{
    public class Interpreter
    {
        private readonly Stack<Dictionary<string,int>> Scopes = new();

        public Interpreter()
        {
            Scopes.Push(new Dictionary<string,int>());
        }

        private void Set(string name, int val)
            => Scopes.Peek()[name] = val;

        private int Get(string name)
        {
            foreach (var sc in Scopes)
                if (sc.ContainsKey(name))
                    return sc[name];
            throw new Exception($"Unknown variable {name}");
        }

        public int Eval(Node n)
        {
            return n switch
            {
                NumberLit num      => num.Value,
                IdentExpr id       => Get(id.Name),

                LetDecl let        => (Set(let.Name, Eval(let.Value)), 0).Item2,

                Binary b           => EvalBin(b),

                ReturnStmt r       => Eval(r.Expr),

                Block blk          => EvalBlock(blk),

                _ => throw new Exception("Unsupported node: " + n)
            };
        }

        private int EvalBlock(Block b)
        {
            Scopes.Push(new Dictionary<string,int>());

            int last = 0;
            foreach (var s in b.Stmts)
                last = Eval(s);

            Scopes.Pop();
            return last;
        }

        private int EvalBin(Binary b)
        {
            int l = Eval(b.L);
            int r = Eval(b.R);

            return b.Op switch
            {
                "+" => l + r,
                "-" => l - r,
                "*" => l * r,
                "/" => r == 0 ? throw new Exception("div0") : l / r,
                _ => throw new Exception("Unknown op: " + b.Op)
            };
        }
    }
}

