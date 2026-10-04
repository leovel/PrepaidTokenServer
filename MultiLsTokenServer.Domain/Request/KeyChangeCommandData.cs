using MultiLsTokenServer.Domain.Request.Base;
using MultiLsTokenServer.Domain.Validation;
using MultiLsTokenServer.Domain.Validation.Rules;
using MultiLsTokenServer.Algorithms.CheckDigit;

namespace MultiLsTokenServer.Domain.Request;

public class KeyChangeCommandData(
    uint keyRegisterOld,
    uint keyRegisterNew,
    string decoderReferenceNumber,
    uint tariffIndexOld,
    uint encryptionAlgorithm,
    uint tokenCarrierType,
    uint tariffIndexNew,
    uint numTokens) : NotifyDataErrorInfo<KeyChangeCommandData>, IKeyRegisterCommandData
{
    public uint KeyRegisterOld { get; } = keyRegisterOld;
    public uint KeyRegister { get; } = keyRegisterNew;
    public string DecoderReferenceNumber { get; } = decoderReferenceNumber;
    public uint TariffIndexOld { get; } = tariffIndexOld;
    public uint EncryptionAlgorithm { get; } = encryptionAlgorithm;
    public uint TokenCarrierType { get; } = tokenCarrierType;
    public uint TariffIndexNew { get; } = tariffIndexNew;
    public uint NumTokens { get; } = numTokens;

    public DateTime IssueDateTime => DateTime.Now;
    public bool RequireValidKey => true;

    public bool IsTidCommand => false;

    static KeyChangeCommandData()
    {
        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field cannot be empty.",
            x => !string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            ));
        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field must be numeric.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || x.DecoderReferenceNumber.IsNumeric()
            ));
        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number must be of 11 or 13 charater length, ",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || x.DecoderReferenceNumber.Length is 11 or 13
            ));
        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number, Luhn Check Digit error.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || !(x.DecoderReferenceNumber.Length is 11 or 13)
            || x.DecoderReferenceNumber == "00000000000"
               || x.DecoderReferenceNumber != "0000000000000"
                   && LuhnCheckDigit.LUHN_CHECK_DIGIT.Calculate(x.DecoderReferenceNumber[..^1]) == $"{x.DecoderReferenceNumber[^1]}"
            ));

        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(EncryptionAlgorithm),
            "Invalid Encryption Algorithm, only 7 (EA07) and 11 (EA11) are allowed.",
            x => x.EncryptionAlgorithm is 7u or 11u
            ));

        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(NumTokens),
            "Invalid Number of Tokens, only 2 or 3 are allowed for EA07.",
            x => x.EncryptionAlgorithm != 7u || x.NumTokens is 2u or 3u
            ));

        Rules.Add(new DelegateRule<KeyChangeCommandData>(
            nameof(NumTokens),
            "Invalid Number of Tokens, only 4 are allowed for EA11.",
            x => x.EncryptionAlgorithm != 11u || x.NumTokens is 4u
            ));
    }
}