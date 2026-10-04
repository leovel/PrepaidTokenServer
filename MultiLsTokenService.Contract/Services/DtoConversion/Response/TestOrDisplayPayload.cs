using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class TestOrDisplayPayload
{
    public static implicit operator TestOrDisplayPayload(MeterTest meterTest)
    {
        return new TestOrDisplayPayload
        {
            Action = meterTest.Action,
            Token = meterTest.Token
        };     
    }

    public static implicit operator MeterTest(TestOrDisplayPayload testOrDisplay)
    {
        return new MeterTest
        {
            Action = testOrDisplay.Action,
            Token = testOrDisplay.Token
        };
    }
}
