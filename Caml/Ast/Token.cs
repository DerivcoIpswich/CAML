namespace Caml.Ast
{
    public struct Token
    {
        public string Value { get; }

        public TokenKind Kind { get; }

        public Token(string value, TokenKind kind)
        {
            Value = value;
            Kind = kind;
        }

        public override string ToString() => Value;
    }
}
