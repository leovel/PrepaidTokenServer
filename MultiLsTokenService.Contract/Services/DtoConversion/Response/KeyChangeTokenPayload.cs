using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class KeyChangeTokenPayload
{
    public static implicit operator KeyChangeTokenPayload(KeyChangeSequence keyChangeSequence)
    {
        return new KeyChangeTokenPayload
        {
            Count = keyChangeSequence.N,
            Token01 = keyChangeSequence.T1,
            Token02 = keyChangeSequence.T2,
            Token03 = keyChangeSequence.T3,
            Token04 = keyChangeSequence.T4
        };
    }

    public static implicit operator KeyChangeSequence(KeyChangeTokenPayload keyChangePayload)
    {
        return new KeyChangeSequence
        {
            N = keyChangePayload.Count,
            T1 = keyChangePayload.Token01,
            T2 = keyChangePayload.Token02,
            T3 = keyChangePayload.Token03,
            T4 = keyChangePayload.Token04
        };
    }
}
