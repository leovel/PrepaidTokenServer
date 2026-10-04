using MultiLsTokenServer.Domain.Exceptions;
using MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities;
using MultiLsTokenServer.Domain.Interfaces.Comunication;
using MultiLsTokenServer.Domain.Request;
using MultiLsTokenServer.Domain.Request.Base;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Header;
using MultiLsTokenServer.Domain.Response.Payloads;
using MultiLsTokenServer.Domain.Validation;
using MultiLsTokenServer.Infrastructure.Tools;

namespace MultiLsTokenServer.Infrastructure.Commands.Base;

using static HsmResponseHeaderFactory;
using static StandardTrasnferSpecificationTools;



public abstract class SecurityModuleService
{
    readonly IAsynchronousHSMClient _asynchronousHSMClient;
    readonly ITidControlService _tidControlService;
    private string? _commandResponse;
    public SecurityModuleService(IAsynchronousHSMClient asynchronousHSMClient, ITidControlService tidControlService)
    {
        _asynchronousHSMClient = asynchronousHSMClient;
        _asynchronousHSMClient.OnDataRecieved = data => { _commandResponse = data; };
        _tidControlService = tidControlService;
    }

    #region COMMON
    protected async Task<HsmResponse<V>> ExecuteCommand<T, V>(T commandData, Func<uint, string, T, string> commandGenerator, Func<string[]?, V?> resultGenerator)
        where V : new()
        where T : NotifyDataErrorInfo<T>
    {

        HsmResponse<V> result = new();
        try
        {
            if (!_asynchronousHSMClient.IsConnected)
            {
                _asynchronousHSMClient.ResetConnection();
                await WaitUntil(() => _asynchronousHSMClient.IsConnected, 30, 3000, false);
            }

            if (_asynchronousHSMClient.IsConnected)
            {
                if (!commandData.HasErrors)
                {
                    VkAttributes keyAttributes = new();
                    string tokenIdentifier = string.Empty;

                    if (commandData is IKeyRegisterCommandData keyRegisterCommandData)
                    {
                        keyAttributes = await GetKeyAttributes(keyRegisterCommandData.KeyRegister);

                        var tidDateTime = _tidControlService.GetTID(keyRegisterCommandData.DecoderReferenceNumber, keyRegisterCommandData.IssueDateTime,
                            keyRegisterCommandData.KeyRegister, keyRegisterCommandData.IsTidCommand);

                        tokenIdentifier = GetTIDBlock(tidDateTime, keyAttributes.BDT, keyRegisterCommandData.IsTidCommand);

                        if (keyRegisterCommandData.RequireValidKey && Convert.ToUInt32(tokenIdentifier, 16) is uint tid && tid >> 16 > keyAttributes.KEN)
                        {
                            result.Header = KeyExpiredHeader($"VK in Register {keyRegisterCommandData.KeyRegister} Expired.");
                            return result;
                        }

                    }

                    var command = commandGenerator(keyAttributes.BDT, tokenIdentifier, commandData);

                    _commandResponse = null;
                    _asynchronousHSMClient.Write(command);

                    await WaitUntil(() => _commandResponse is not null, 50, 5000);

                    if (!string.IsNullOrEmpty(_commandResponse))
                    {
                        var splitedResponse = _commandResponse?.Split('~');

                        var code = splitedResponse?[0][5..7] ?? UnknownCode;
                        result.Header = CreateHeaderFromCode(code);

                        if (!result.IsError)
                            result.Payload = resultGenerator(splitedResponse) ?? new();
                    }
                }
                else
                {
                    result.Header = ParameterValidationError(string.Join(Environment.NewLine, commandData.GetErrors()));
                }
            }
            else
            {
                result.Header = NoConnectionHeader();
            }
        }
        catch (Exception ex)
        {
            result.Header = ex switch
            {
                ConnectionException => NoConnectionHeader(ex.Message),
                ParameterOutOfRangeException => ParameterOutOfRangeHeader(ex.Message),
                NoHsmResponseException => NoHsmResponseHeader(ex.Message),
                _ => ResponseGenerationExceptionHeader(ex.Message),
            };
        }

        return result;
    }

    protected async Task<HsmResponse<Token>> VendSTSToken(string requestID, TokenCommandData commandData)
    {
        return await ExecuteCommand(commandData,
            (baseDate, tokenIdentifier, commandData) =>
            {
                string meterPan = GetMeterPan(commandData.DecoderReferenceNumber);

                string ammount = GetAmountBlock(commandData.TransferAmmount, commandData.Class, commandData.SubClass);


                string command =
                    $"{requestID}N{commandData.KeyRegister}~P{meterPan}~N{commandData.TariffIndex}~N{commandData.EncryptionAlgorithm}~N{commandData.TokenCarrierType}~N{commandData.SubClass}~H{ammount}~H{tokenIdentifier}~";
                return command;
            },
            splitedResponse =>
            {
                Token token = FormatToken(splitedResponse?[1][1..] ?? string.Empty);
                return token;
            });
    }

    private static readonly Dictionary<uint, (DateTime time, VkAttributes vkAttributes)> keyAttributesCache = [];
    protected async Task<VkAttributes> GetKeyAttributes(uint keyRegister)
    {
        if (keyAttributesCache.TryGetValue(keyRegister, out (DateTime time, VkAttributes vkAttributes) cachedValue)
            && (DateTime.Now - cachedValue.time).TotalMinutes < 120.0)
            return cachedValue.vkAttributes;

        _commandResponse = null;
        _asynchronousHSMClient.Write($"SM?GAN{keyRegister}~");

        await WaitUntil(() => _commandResponse is not null, 50, 5000);

        if (!string.IsNullOrEmpty(_commandResponse))
        {
            var splitedResponse = _commandResponse?.Split('~');

            var code = splitedResponse?[0][5..7] ?? UnknownCode;
            var header = CreateHeaderFromCode(code);

            if (!header.IsError)
            {
                var attributes = splitedResponse?[0].Split(';') ?? [];
                var baseDateStr = attributes[1][3..7] ?? string.Empty;
                var kenStr = attributes.FirstOrDefault(s => s.StartsWith("KEN"))?[3..] ?? string.Empty;

                if (uint.TryParse(baseDateStr, out uint baseDateValue) && uint.TryParse(kenStr, out uint kenValue))
                {
                    _commandResponse = null;

                    var result = new VkAttributes(baseDateValue, kenValue);
                    keyAttributesCache[keyRegister] = (DateTime.Now, result);

                    return result;
                }
                else
                {
                    _commandResponse = null;
                    throw new KeyValidationException("Key metadata error: Conversion Error.");
                }

            }
            else
            {
                _commandResponse = null;
                throw new KeyValidationException(header.Description);
            }

        }
        else
        {
            _commandResponse = null;
            throw new KeyValidationException("Key metadata error: No response.");
        }
    }
    #endregion
}
