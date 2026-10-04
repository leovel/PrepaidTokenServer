using MultiLsTokenServer.Domain.Request.Base;
using MultiLsTokenServer.Domain.Validation;
using MultiLsTokenServer.Domain.Validation.Rules;
using MultiLsTokenServer.Algorithms.CheckDigit;

namespace MultiLsTokenServer.Domain.Request;

public class TokenCommandData(
    uint keyRegister,
    string decoderReferenceNumber,
    uint tariffIndex,
    uint encryptionAlgorithm,
    uint tokenCarrierType,
    double transferAmmount,
    DateTime issueDateTime) : NotifyDataErrorInfo<TokenCommandData>, IKeyRegisterCommandData
{
    public uint KeyRegister { get; } = keyRegister;
    public string DecoderReferenceNumber { get; } = decoderReferenceNumber;
    public uint TariffIndex { get; } = tariffIndex;
    public uint EncryptionAlgorithm { get; } = encryptionAlgorithm;
    public uint TokenCarrierType { get; } = tokenCarrierType;
    public uint Class { get; set; }
    public uint SubClass { get; set; }
    public double TransferAmmount { get; } = transferAmmount;
    public DateTime IssueDateTime { get; } = issueDateTime;

    public bool RequireValidKey => true;

    public bool IsTidCommand => true;


    static TokenCommandData()
    {
        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field cannot be empty.",
            x => !string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            ));
        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field must be numeric.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || x.DecoderReferenceNumber.IsNumeric()
            ));
        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number must be of 11 or 13 charater length, ",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || x.DecoderReferenceNumber.Length is 11 or 13
            ));
        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number, Luhn Check Digit error.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || !(x.DecoderReferenceNumber.Length is 11 or 13)
            || x.DecoderReferenceNumber == "00000000000"
               || x.DecoderReferenceNumber != "0000000000000"
                   && LuhnCheckDigit.LUHN_CHECK_DIGIT.Calculate(x.DecoderReferenceNumber[..^1]) == $"{x.DecoderReferenceNumber[^1]}"
            ));

        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(EncryptionAlgorithm),
            "Invalid Encryption Algorithm, only 7 and 11 are allowed.",
            x => x.EncryptionAlgorithm is 7u or 11u
            ));

        Rules.Add(new DelegateRule<TokenCommandData>(
            nameof(TransferAmmount),
            "Invalid Transfer Ammount (Register to Clear), must be a 16 bits integer.",
            x => x.Class != 2u || x.SubClass != 1u || ushort.TryParse($"{x.TransferAmmount}", out _)
            ));
    }
}
