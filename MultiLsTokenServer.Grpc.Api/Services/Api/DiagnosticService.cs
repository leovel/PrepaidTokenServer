using Grpc.Core;
using MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;

namespace MultiLsTokenServer.Grpc.Api.Services;

public class DiagnosticService(IHsmDiagnosticService hsmDiagnosticService) : DiagnosticApi.DiagnosticApiBase
{
    public override async Task<CheckSecurityModuleResponse> CheckSecurityModule(EmptyRequest request, ServerCallContext context)
    {
        var statusResponse = await hsmDiagnosticService.SecurityModuleConnected();
        return new CheckSecurityModuleResponse { Header = statusResponse.Header, Status = statusResponse.Payload };
    }

    public override async Task<GetIdentificationResponse> GetIdentification(EmptyRequest request, ServerCallContext context)
    {
        var identificationResponse = await hsmDiagnosticService.GetIdentification();
        return new GetIdentificationResponse { Header = identificationResponse.Header, Identification = identificationResponse.Payload };
    }

    public override async Task<QueryIdentificationResponse> QueryIdentification(EmptyRequest request, ServerCallContext context)
    {
        var queryIdentificationResponse = await hsmDiagnosticService.GetQueryIdentification();
        return new QueryIdentificationResponse { Header = queryIdentificationResponse.Header, QueryIdentification = queryIdentificationResponse.Payload };
    }

    public override async Task<QueryDateResponse> QueryDate(EmptyRequest request, ServerCallContext context)
    {
        var queryDateResponse = await hsmDiagnosticService.GetQueryDate();
        return new QueryDateResponse { Header = queryDateResponse.Header, QueryDate = queryDateResponse.Payload };
    }
}
