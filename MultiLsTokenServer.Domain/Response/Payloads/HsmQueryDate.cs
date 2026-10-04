namespace MultiLsTokenServer.Domain.Response.Payloads;

public record HsmQueryDate(DateTime RtcTime, uint WindowSize)
{
    public HsmQueryDate() : this(default, 0) { }
}
