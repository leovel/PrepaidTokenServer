namespace MultiLsTokenServer.Domain.Response.Payloads;

public record KeyChangeSequence(uint N, Token T1, Token T2, Token T3, Token T4)
{
    public KeyChangeSequence() : this(0, string.Empty, string.Empty, string.Empty, string.Empty) { }
}
