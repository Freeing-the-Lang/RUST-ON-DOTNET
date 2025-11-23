// File: parser.cs

using System;
using System.Collections.Generic;

namespace RustOnDotnet
{
    public class Parser
    {
        private readonly List<Token> _toks;
        private int _i = 0;

        public Parser(List<Token> toks) => _toks = toks;

        private Token Peek() => _toks[_i];
        private Token Next() => _toks[_i++];

        public Node Parse()
        {
            var t = Peek();

            if (t.Kind == TokenKind.Keyword && t.Text == "let")
                return ParseLet();

            return ParseExpr();
        }

        private Node ParseLet()
        {
            Next(); // let
            string name = Next().Text; // identifier
            Next(); // '='
            Node val = ParseExpr();
            return new LetDecl(name, val);
        }

        private Node ParseExpr()
        {
            Node left = ParsePrimary();

            while (Peek().Kind == TokenKind.Symbol && Peek().Text == "+")
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
    }
}
