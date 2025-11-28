using System;
using System.Collections.Generic;

namespace RustOnDotnet
{
    public class Parser
    {
        private readonly List<Token> _t;
        private int _i = 0;

        public Parser(List<Token> toks) => _t = toks;

        private Token Peek() => _t[_i];
        private Token Next() => _t[_i++];

        private bool Is(string s) => Peek().Text == s;
        private bool Is(TokenKind k) => Peek().Kind == k;

        private bool End => _i >= _t.Count;

        public Node Parse()
        {
            if (Is("fn")) return ParseFn();
            return ParseStmt();
        }

        // ---------------------------
        // fn <name>() { block }
        // ---------------------------
        private Node ParseFn()
        {
            Next(); // fn
            string name = Next().Text;

            Expect("(");
            Expect(")");

            Block body = ParseBlock();

            return new FnDecl(name, new List<string>(), body);
        }

        // ---------------------------
        // { ... }
        // ---------------------------
        private Block ParseBlock()
        {
            Expect("{");
            var list = new List<Node>();

            while (!Is("}") && !End)
                list.Add(ParseStmt());

            Expect("}");
            return new Block(list);
        }

        // ---------------------------
        // let / return / expr
        // ---------------------------
        private Node ParseStmt()
        {
            if (Is("let")) return ParseLet();
            if (Is("return")) return ParseReturn();
            return ParseExpr();
        }

        private Node ParseLet()
        {
            Next(); // let
            string name = Next().Text;
            Expect("=");
            Node val = ParseExpr();
            return new LetDecl(name, val);
        }

        private Node ParseReturn()
        {
            Next(); // return
            Node value = ParseExpr();
            return new ReturnStmt(value);
        }

        // ============================================================
        // Expression Parser (with precedence!)
        // ============================================================
        //
        // Expr  → Term ( ("+"|"-") Term )*
        // Term  → Factor ( ("*"|"/") Factor )*
        // Factor → Number | Ident | "(" Expr ")"
        //
        // ============================================================

        private Node ParseExpr()
        {
            Node node = ParseTerm();

            while (Is("+") || Is("-"))
            {
                string op = Next().Text;
                Node r = ParseTerm();
                node = new Binary(op, node, r);
            }

            return node;
        }

        private Node ParseTerm()
        {
            Node node = ParseFactor();

            while (Is("*") || Is("/"))
            {
                string op = Next().Text;
                Node r = ParseFactor();
                node = new Binary(op, node, r);
            }

            return node;
        }

        private Node ParseFactor()
        {
            Token t = Peek();

            // (expr)
            if (Is("("))
            {
                Next();
                Node e = ParseExpr();
                Expect(")");
                return e;
            }

            Next(); // consume token

            return t.Kind switch
            {
                TokenKind.Number => new NumberLit(int.Parse(t.Text)),
                TokenKind.Ident => new IdentExpr(t.Text),
                _ => throw new Exception("Unexpected token: " + t.Text)
            };
        }

        private void Expect(string s)
        {
            if (Next().Text != s)
                throw new Exception($"Expected '{s}'");
        }
    }
}
