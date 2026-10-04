using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;

public interface IHsmDiagnosticService
{
    Task<HsmResponse<bool>> SecurityModuleConnected();

    Task<HsmResponse<HsmIdentification>> GetIdentification();
    Task<HsmResponse<HsmQueryIdentification>> GetQueryIdentification();
    Task<HsmResponse<HsmQueryDate>> GetQueryDate();
}
