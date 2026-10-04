using Grpc.Core;
using MultiLsTokenServer.Domain.Interfaces.Commands.TestDisplay;

namespace MultiLsTokenServer.Grpc.Api.Services;

public class TestDisplayService(IMeterTestDisplayService meterTestDisplayService) : TestOrDisplayApi.TestOrDisplayApiBase
{
    public override async Task<TestOrDisplayResponse> GenerateMeterTestOrDisplayControFieldToken(TestOrDisplayRequest request, ServerCallContext context)
    {
        var testDisplayResponse = await Task.FromResult(meterTestDisplayService.GetTestDisplayToken(request));
        return new TestOrDisplayResponse { Header = testDisplayResponse.Header, TestOrDisplay = testDisplayResponse.Payload };
    }
}
