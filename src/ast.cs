// File: ast.cs

using System.Collections.Generic;

namespace RustOnDotnet
{
    public abstract class Node {}

    public class NumberLit : Node
    {
        public int Value;
        public NumberLit(int v) => Value = v;
    }

    public class IdentExpr : Node
    {
        public string Name;
        public IdentExpr(string name) => Name = name;
    }

    public class Binary : Node
    {
        public string Op;
        public Node Left;
        public Node Right;

        public Binary(string op, Node l, Node r)
        {
            Op = op;
            Left = l;
            Right = r;
        }
    }

    public class LetDecl : Node
    {
        public string Name;
        public Node Value;

        public LetDecl(string name, Node val)
        {
            Name = name;
            Value = val;
        }
    }

    public class FnDecl : Node
    {
        public string Name;
        public List<string> Args;
        public Node Body;

        public FnDecl(string n, List<string> a, Node b)
        {
            Name = n;
            Args = a;
            Body = b;
        }
    }
}
