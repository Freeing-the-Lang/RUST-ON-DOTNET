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
        public IdentExpr(string n) => Name = n;
    }

    public class Binary : Node
    {
        public string Op;
        public Node L;
        public Node R;
        public Binary(string op, Node l, Node r)
        {
            Op = op;
            L = l;
            R = r;
        }
    }

    public class LetDecl : Node
    {
        public string Name;
        public Node Value;
        public LetDecl(string n, Node v) { Name = n; Value = v; }
    }

    public class ReturnStmt : Node
    {
        public Node Expr;
        public ReturnStmt(Node e) => Expr = e;
    }

    public class Block : Node
    {
        public List<Node> Stmts;
        public Block(List<Node> s) => Stmts = s;
    }

    public class FnDecl : Node
    {
        public string Name;
        public List<string> Args;
        public Block Body;

        public FnDecl(string n, List<string> a, Block b)
        {
            Name = n; Args = a; Body = b;
        }
    }
}
