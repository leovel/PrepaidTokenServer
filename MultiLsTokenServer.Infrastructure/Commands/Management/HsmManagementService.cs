using MultiLsTokenServer.Infrastructure.Commands.Base;
using MultiLsTokenServer.Domain.Exceptions;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Commands.Management;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;
using MultiLsTokenServer.Infrastructure.Tools;

namespace MultiLsTokenServer.Infrastructure.Commands.Management;

using static StandardTrasnferSpecificationTools;

public class HsmManagementService(IAsynchronousHSMClient asynchronousHSMClient, ITidControlService tidControlService) : SecurityModuleService(asynchronousHSMClient, tidControlService), IHsmManagementService
{
    protected async Task<HsmResponse<Token>> VendSTSManagementToken(TokenCommandData commandData)
    {
        string requestID = "SM?VM";
        commandData.Class = 2;

        return await VendSTSToken(requestID, commandData);
    }

    #region MANAGEMENT
    public async Task<HsmResponse<Token>> SetMaximumPowerLimit(TokenCommandData commandData)
    {
        commandData.SubClass = 0;
        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<Token>> ClearCredit(TokenCommandData commandData)
    {
        commandData.SubClass = 1;

        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<Token>> SetTariffRate(TokenCommandData commandData)
    {
        commandData.SubClass = 2;
        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<Token>> ClearTamperCondition(TokenCommandData commandData)
    {
        commandData.SubClass = 5;
        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<Token>> SetMaximumPhasePowerUnbalanceLimit(TokenCommandData commandData)
    {
        commandData.SubClass = 6;
        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<Token>> SetWaterMeterFactor(TokenCommandData commandData)
    {
        commandData.SubClass = 7;
        return await VendSTSManagementToken(commandData);
    }

    public async Task<HsmResponse<KeyChangeSequence>> KeyChangeToken(KeyChangeCommandData commandData)
    {
        return await ExecuteCommand(commandData,
        (baseDate, tokenIdentifier, commandData) =>
        {
            string requestID = "SM?VK";

            string meterPan = GetMeterPan(commandData.DecoderReferenceNumber);

            string command =
                $"{requestID}N{commandData.KeyRegisterOld}~N{commandData.KeyRegister}~P{meterPan}~N{commandData.TariffIndexOld}~N{commandData.EncryptionAlgorithm}~N{commandData.TokenCarrierType}~N{commandData.TariffIndexNew}~N{commandData.NumTokens}~";
            return command;
        },
            splitedResponse =>
            {
                var tokens = splitedResponse?.Length > 3 ? FormatTokens(splitedResponse[3][1..]) : [];

                uint n = (uint)tokens.Count;
                Token t1 = n > 0 ? tokens[0] : string.Empty;
                Token t2 = n > 1 ? tokens[1] : string.Empty;
                Token t3 = n > 2 ? tokens[2] : string.Empty;
                Token t4 = n > 3 ? tokens[3] : string.Empty;

                return new KeyChangeSequence(n, t1, t2, t3, t4);
            });
    }

    public async Task<HsmResponse<TokenVerification>> VerifySTSToken(TokenVerificationCommandData commandData)
    {
        uint keyBaseDate = 0;
        return await ExecuteCommand(commandData,
            (baseDate, tokenIdentifier, commandData) =>
            {
                keyBaseDate = baseDate;
                string requestID = "SM?VT";

                string meterPan = GetMeterPan(commandData.DecoderReferenceNumber);
                string command =
                    $"{requestID}N{commandData.KeyRegister}~P{meterPan}~N{commandData.TariffIndex}~N{commandData.EncryptionAlgorithm}~P{commandData.TokenDec.Replace("-", "").Replace(" ", "")}~";
                return command;
            },
            splitedResponse =>
            {
                bool valid = splitedResponse is not null
                    && splitedResponse.Length >= 5
                    && splitedResponse[0].Last() == '0';

                uint tokenClass = 0;
                uint subClass = 0;
                double transferAmmount = 0.0;
                DateTime issueDateTime = default;

                if (valid)
                {
                    tokenClass = uint.Parse($"{splitedResponse![1][1]}");
                    subClass = uint.Parse($"{splitedResponse![2][1]}");
                    transferAmmount = GetTransferAmount(splitedResponse![3][1..], tokenClass, subClass);
                    issueDateTime = GetIssueDateTime(splitedResponse![4][1..], keyBaseDate);
                }

                return new TokenVerification(valid, tokenClass, subClass, transferAmmount, issueDateTime);
            });
    }
    #endregion
}
