using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class TestOrDisplayRequest
{
    public static implicit operator MeterTestCommandData(TestOrDisplayRequest request)
    {
        return new MeterTestCommandData(
            request.DecoderReferenceNumber,
            request.TestNumber);
    }

    public static implicit operator TestOrDisplayRequest(MeterTestCommandData request)
    {
        return new TestOrDisplayRequest
        {
            DecoderReferenceNumber = request.DecoderReferenceNumber,
            TestNumber = request.TestNumber
        };
    }
}
