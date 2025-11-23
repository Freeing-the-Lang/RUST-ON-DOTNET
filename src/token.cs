using System;

namespace RustOnDotnet
{
    public enum TokenKind
    {
        Ident,
        Number,
        Keyword,
        Symbol,
        EOF
    }

    public class Token
    {
        public TokenKind Kind { get; }
        public string Text { get; }

        public Token(TokenKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public override string ToString()
            => $"{Kind}:{Text}";
    }
}
