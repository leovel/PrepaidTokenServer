namespace MultiLsTokenServer.Domain.Response.Payloads;

public record MeterTest(string Action, Token Token)
{
    public MeterTest() : this(string.Empty, string.Empty) { }
}
