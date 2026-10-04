using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Domain.Interfaces.Commands.TestDisplay;

public interface IMeterTestDisplayService
{
    HsmResponse<MeterTest> GetTestDisplayToken(MeterTestCommandData commandData);
}
