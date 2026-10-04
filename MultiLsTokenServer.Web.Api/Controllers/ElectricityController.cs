using Microsoft.AspNetCore.Mvc;
using MultiLsTokenServer.Domain.Interfaces.Commands.Vending;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsVendingSystem.Api.Controllers;

[Route("prepaid/[controller]")]
[ApiController]
public class ElectricityController(IHsmVendingElectricityService service) : ControllerBase
{
    [HttpPost("credit")]
    public async Task<HsmResponse<Token>> GenerateTransferCreditToken(TokenCommandData commandData)
    {
        return await service.TransferCurrencyToken(commandData);
    }
}
