using Grpc.Core;
using MultiLsTokenServer.Domain.Interfaces.Commands.Vending;

namespace MultiLsTokenServer.Grpc.Api.Services;

public class VendingGasService(IHsmVendingGasService hsmVendingService)
    : VendingGasApi.VendingGasApiBase
{
    public override async Task<TokenResponse> GenerateTransferCreditToken(TransferCreditTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmVendingService.TransferCreditToken(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateTransferCurrencyToken(TransferCreditTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmVendingService.TransferCurrencyToken(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }
}
