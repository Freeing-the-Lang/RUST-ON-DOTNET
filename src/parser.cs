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

        public Node Parse()
        {
            if (Is("fn")) return ParseFn();
            return ParseStmt();
        }

        private bool Is(string s)
            => Peek().Text == s;

        private Node ParseFn()
        {
            Next(); // fn
            string name = Next().Text;

            Expect("(");
            Expect(")");

            Block body = ParseBlock();

            return new FnDecl(name, new List<string>(), body);
        }

        private Block ParseBlock()
        {
            Expect("{");
            var list = new List<Node>();

            while (!Is("}"))
                list.Add(ParseStmt());

            Expect("}");
            return new Block(list);
        }

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
            Next();
            return new ReturnStmt(ParseExpr());
        }

        private Node ParseExpr()
        {
            Node left = ParsePrimary();

            while (Is("+") || Is("-") || Is("*") || Is("/"))
            {
                string op = Next().Text;
                Node right = ParsePrimary();
                left = new Binary(op, left, right);
            }

            return left;
        }

        private Node ParsePrimary()
        {
            Token t = Next();

            return t.Kind switch
            {
                TokenKind.Number => new NumberLit(int.Parse(t.Text)),
                TokenKind.Ident => new IdentExpr(t.Text),
                _ => throw new Exception("Unexpected token: " + t)
            };
        }

        private void Expect(string s)
        {
            if (Next().Text != s)
                throw new Exception($"Expected '{s}'");
        }
    }
}

