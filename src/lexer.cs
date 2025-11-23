// File: lexer.cs

using System;
using System.Collections.Generic;

namespace RustOnDotnet
{
    public class Lexer
    {
        private readonly string _src;
        private int _i = 0;

        private static readonly HashSet<string> Keywords = new()
        {
            "fn", "let", "mut", "return"
        };

        public Lexer(string src) => _src = src;

        public Token Next()
        {
            Skip();

            if (_i >= _src.Length)
                return new Token(TokenKind.EOF, "");

            char c = _src[_i];

            if (char.IsLetter(c))
                return Ident();

            if (char.IsDigit(c))
                return Number();

            return new Token(TokenKind.Symbol, _src[_i++].ToString());
        }

        private Token Ident()
        {
            int s = _i;
            while (_i < _src.Length && char.IsLetterOrDigit(_src[_i]))
                _i++;

            string word = _src[s.._i];
            if (Keywords.Contains(word))
                return new Token(TokenKind.Keyword, word);
            return new Token(TokenKind.Ident, word);
        }

        private Token Number()
        {
            int s = _i;
            while (_i < _src.Length && char.IsDigit(_src[_i]))
                _i++;

            return new Token(TokenKind.Number, _src[s.._i]);
        }

        private void Skip()
        {
            while (_i < _src.Length && char.IsWhiteSpace(_src[_i]))
                _i++;
        }
    }
}
