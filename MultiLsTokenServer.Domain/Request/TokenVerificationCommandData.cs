using MultiLsTokenServer.Domain.Request.Base;
using MultiLsTokenServer.Domain.Validation;
using MultiLsTokenServer.Domain.Validation.Rules;
using MultiLsTokenServer.Algorithms.CheckDigit;

namespace MultiLsTokenServer.Domain.Request;

public class TokenVerificationCommandData(
    uint keyRegister,
    string decoderReferenceNumber,
    uint tariffIndex,
    uint encryptionAlgorithm,
    string tokenDec) : NotifyDataErrorInfo<TokenVerificationCommandData>, IKeyRegisterCommandData
{
    public uint KeyRegister { get; } = keyRegister;
    public string DecoderReferenceNumber { get; } = decoderReferenceNumber;
    public uint TariffIndex { get; } = tariffIndex;
    public uint EncryptionAlgorithm { get; } = encryptionAlgorithm;
    public string TokenDec { get; } = tokenDec;

    public DateTime IssueDateTime => DateTime.Now;
    public bool RequireValidKey => false;

    public bool IsTidCommand => false;

    static TokenVerificationCommandData()
    {
        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field cannot be empty.",
            x => !string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            ));
        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field must be numeric.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || x.DecoderReferenceNumber.IsNumeric()
            ));
        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number must be of 11 or 13 charater length, ",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || x.DecoderReferenceNumber.Length is 11 or 13
            ));
        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number, Luhn Check Digit error.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || !(x.DecoderReferenceNumber.Length is 11 or 13)
            || x.DecoderReferenceNumber == "00000000000"
               || x.DecoderReferenceNumber != "0000000000000"
                   && LuhnCheckDigit.LUHN_CHECK_DIGIT.Calculate(x.DecoderReferenceNumber[..^1]) == $"{x.DecoderReferenceNumber[^1]}"
            ));

        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(EncryptionAlgorithm),
            "Invalid Encryption Algorithm, only 7 (EA07) and 11 (EA11) are allowed.",
            x => x.EncryptionAlgorithm is 7u or 11u
            ));

        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(TokenDec),
            "The Token field cannot be empty.",
            x => !string.IsNullOrWhiteSpace(x.TokenDec)
            ));

        Rules.Add(new DelegateRule<TokenVerificationCommandData>(
            nameof(TokenDec),
            "Invalid Token Format.",
            x => string.IsNullOrWhiteSpace(x.TokenDec)
            || x.TokenDec.Replace("-", "").Replace(" ", "") is string token && token.IsNumeric() && token.Length == 20

            ));
    }
}

