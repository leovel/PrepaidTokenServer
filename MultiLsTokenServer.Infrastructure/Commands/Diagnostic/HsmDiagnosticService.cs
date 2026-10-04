using MultiLsTokenServer.Infrastructure.Commands.Base;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Commands.Diagnostic;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Payloads;
using MultiLsTokenServer.Domain;

namespace MultiLsTokenServer.Infrastructure.Commands.Diagnostic;

public class HsmDiagnosticService(IAsynchronousHSMClient asynchronousHSMClient, ITidControlService tidControlService) : SecurityModuleService(asynchronousHSMClient, tidControlService), IHsmDiagnosticService
{
    #region DIAGNOSTIC
    public async Task<HsmResponse<bool>> SecurityModuleConnected()
    {
        return await ExecuteCommand(new RawCommandData("GL?EC0000512345"),
            (baseDate, tokenIdentifier, commandData) => commandData.Command,
            splitedResponse =>
            {
                return splitedResponse?[0][10..15] == "12345";
            });
    }

    public async Task<HsmResponse<HsmIdentification>> GetIdentification()
    {
        return await ExecuteCommand(new RawCommandData("SM?DI"),
            (baseDate, tokenIdentifier, commandData) => commandData.Command,
            splitedResponse =>
            {
                var moduleIdentifier = splitedResponse?[0][8..] ?? string.Empty;
                var firmaIdentifier = splitedResponse?[1][1..] ?? string.Empty;

                return new HsmIdentification(moduleIdentifier, firmaIdentifier);
            });
    }

    public async Task<HsmResponse<HsmQueryIdentification>> GetQueryIdentification()
    {
        return await ExecuteCommand(new RawCommandData("SM?QI"),
            (baseDate, tokenIdentifier, commandData) => commandData.Command,
            splitedResponse =>
            {
                var PublicKeyIdentifier = splitedResponse?[0][8..] ?? string.Empty;
                var HardwareIdentifier = splitedResponse?[1][1..] ?? string.Empty;
                var FirmwareIdentifier = splitedResponse?[2][1..] ?? string.Empty;
                var FirmwareHash = splitedResponse?[3][1..] ?? string.Empty;

                return new HsmQueryIdentification(PublicKeyIdentifier, HardwareIdentifier, FirmwareIdentifier, FirmwareHash);
            });
    }

    public async Task<HsmResponse<HsmQueryDate>> GetQueryDate()
    {
        return await ExecuteCommand(new RawCommandData("SM?QD"),
            (baseDate, tokenIdentifier, commandData) => commandData.Command,
            splitedResponse =>
            {
                _ = (splitedResponse?[0][8..] ?? string.Empty).TryConvertToDateTime(out DateTime rtcTime);
                _ = uint.TryParse(splitedResponse?[1][1..], out uint windowSize);

                return new HsmQueryDate(rtcTime, windowSize);
            });
    }
    #endregion
}
