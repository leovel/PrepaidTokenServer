using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class QueryDatePayload
{
    public static implicit operator QueryDatePayload(HsmQueryDate queryDate)
    {
        return new QueryDatePayload
        {
            RtcTime = queryDate.RtcTime,
            WindowSize = queryDate.WindowSize
        };
    }

    public static implicit operator HsmQueryDate(QueryDatePayload queryDate)
    {
        return new HsmQueryDate
        {
            RtcTime = queryDate.RtcTime,
            WindowSize = queryDate.WindowSize
        };
    }
}
