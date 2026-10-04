using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Domain.Interfaces.Commands.Management;

public interface IHsmManagementService
{
    Task<HsmResponse<Token>> SetMaximumPowerLimit(TokenCommandData commandData);
    Task<HsmResponse<Token>> ClearCredit(TokenCommandData commandData);
    Task<HsmResponse<Token>> SetTariffRate(TokenCommandData commandData);
    Task<HsmResponse<Token>> ClearTamperCondition(TokenCommandData commandData);
    Task<HsmResponse<Token>> SetMaximumPhasePowerUnbalanceLimit(TokenCommandData commandData);
    Task<HsmResponse<Token>> SetWaterMeterFactor(TokenCommandData commandData);

    Task<HsmResponse<KeyChangeSequence>> KeyChangeToken(KeyChangeCommandData commandData);

    Task<HsmResponse<TokenVerification>> VerifySTSToken(TokenVerificationCommandData commandData);
}
