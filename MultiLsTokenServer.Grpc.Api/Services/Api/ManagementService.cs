using Grpc.Core;
using MultiLsTokenServer.Domain.Interfaces.Commands.Management;

namespace MultiLsTokenServer.Grpc.Api.Services;

public class ManagementService(IHsmManagementService hsmManagementService) : ManagementApi.ManagementApiBase
{
    public override async Task<TokenResponse> GenerateSetMaximumPowerLimitToken(ManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.SetMaximumPowerLimit(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateClearCreditToken(ManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.ClearCredit(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateSetTariffRateToken(ManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.SetTariffRate(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateClearTamperConditionToken(NoAmmountManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.ClearTamperCondition(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateSetMaximumPhasePowerUnbalanceLimitToken(ManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.SetMaximumPhasePowerUnbalanceLimit(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<TokenResponse> GenerateSetWaterMeterFactorToken(ManagementTokenRequest request, ServerCallContext context)
    {
        var tokenResponse = await hsmManagementService.SetWaterMeterFactor(request);
        return new TokenResponse { Header = tokenResponse.Header, Token = tokenResponse.Payload };
    }

    public override async Task<KeyChangeTokenResponse> GenerateKeyChangeToken(KeyChangeTokenRequest request, ServerCallContext context)
    {
        var keyChangeResponse = await hsmManagementService.KeyChangeToken(request);
        return new KeyChangeTokenResponse { Header = keyChangeResponse.Header, KeyChangeSequence = keyChangeResponse.Payload };
    }

    public override async Task<VerifyTokenResponse> VerifyToken(VerifyTokenRequest request, ServerCallContext context)
    {
        var verifyTokenResponse = await hsmManagementService.VerifySTSToken(request);
        return new VerifyTokenResponse { Header = verifyTokenResponse.Header, TokenVerification = verifyTokenResponse.Payload };
    }
}
