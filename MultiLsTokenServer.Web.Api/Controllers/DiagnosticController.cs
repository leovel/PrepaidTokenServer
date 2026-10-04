using Microsoft.AspNetCore.Mvc;
using MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsVendingSystem.Api.Controllers;

[Route("prepaid/[controller]")]
[ApiController]
public class DiagnosticController(IHsmDiagnosticService service) : ControllerBase
{
    [HttpGet("online")]
    public async Task<HsmResponse<bool>> CheckSecurityModule()
    {
        return await service.SecurityModuleConnected();
    }

    [HttpGet("identification")]
    public async Task<HsmResponse<HsmIdentification>> GetIdentification()
    {
        return await service.GetIdentification();
    }

    [HttpGet("query-identification")]
    public async Task<HsmResponse<HsmQueryIdentification>> QueryIdentification()
    {
        return await service.GetQueryIdentification();
    }

    [HttpGet("date")]
    public async Task<HsmResponse<HsmQueryDate>> QueryDate()
    {
        return await service.GetQueryDate();
    }
}
