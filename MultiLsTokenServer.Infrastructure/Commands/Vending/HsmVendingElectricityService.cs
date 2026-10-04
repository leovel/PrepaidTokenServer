using MultiLsTokenServer.Infrastructure.Commands.Vending.Base;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Commands.Vending;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Infrastructure.Commands.Vending;

public class HsmVendingElectricityService(IAsynchronousHSMClient asynchronousHSMClient, ITidControlService tidControlService)
    : HsmVendingService(asynchronousHSMClient, tidControlService), IHsmVendingElectricityService
{

    #region VENDING CREDIT
    public async Task<HsmResponse<Token>> TransferCreditToken(TokenCommandData commandData)
    {
        commandData.SubClass = 0;

        return await SendVendingCommand(commandData);
    }
    #endregion

    #region VENDING CURRENCY
    public async Task<HsmResponse<Token>> TransferCurrencyToken(TokenCommandData commandData)
    {
        commandData.SubClass = 4;

        return await SendVendingCommand(commandData);
    }
    #endregion

}
