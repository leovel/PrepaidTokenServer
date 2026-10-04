namespace MultiLsTokenServer.Domain.Response.Payloads;

public record Token(string Value)
{
    public Token() : this(string.Empty) { }

    public static implicit operator string(Token token)
    {
        return token.Value;
    }

    public static implicit operator Token(string value)
    {
        return new Token(value);
    }

    public override string ToString() => Value;
}
