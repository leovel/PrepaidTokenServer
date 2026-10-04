using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Domain.Interfaces.Commands.Vending.Base;

public interface IHsmVendingService
{
    Task<HsmResponse<Token>> TransferCreditToken(TokenCommandData commandData);
    Task<HsmResponse<Token>> TransferCurrencyToken(TokenCommandData commandData);
}
