using MultiLsTokenServer.Infrastructure.Commands.Base;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Infrastructure.Commands.Vending.Base;

public abstract class HsmVendingService(IAsynchronousHSMClient asynchronousHSMClient, ITidControlService tidControlService) : SecurityModuleService(asynchronousHSMClient, tidControlService)
{

    #region COMMON
    protected async Task<HsmResponse<Token>> SendVendingCommand(TokenCommandData commandData)
    {
        string requestID = "SM?VC";
        commandData.Class = 0;

        return await VendSTSToken(requestID, commandData);
    }
    #endregion
}
