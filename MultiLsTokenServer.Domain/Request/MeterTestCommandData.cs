using MultiLsTokenServer.Domain.Validation;
using MultiLsTokenServer.Domain.Validation.Rules;
using MultiLsTokenServer.Algorithms.CheckDigit;

namespace MultiLsTokenServer.Domain.Request;

public class MeterTestCommandData(
    string decoderReferenceNumber,
    uint testNumber) : NotifyDataErrorInfo<MeterTestCommandData>
{
    public string DecoderReferenceNumber { get; } = decoderReferenceNumber;
    public uint TestNumber { get; } = testNumber;


    static MeterTestCommandData()
    {
        Rules.Add(new DelegateRule<MeterTestCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field cannot be empty.",
            x => !string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            ));
        Rules.Add(new DelegateRule<MeterTestCommandData>(
            nameof(DecoderReferenceNumber),
            "The Decoder Reference Number field must be numeric.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || x.DecoderReferenceNumber.IsNumeric()
            ));
        Rules.Add(new DelegateRule<MeterTestCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number must be of 11 or 13 charater length, ",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || x.DecoderReferenceNumber.Length is 11 or 13
            ));
        Rules.Add(new DelegateRule<MeterTestCommandData>(
            nameof(DecoderReferenceNumber),
            "Invalid Decoder Reference Number, Luhn Check Digit error.",
            x => string.IsNullOrWhiteSpace(x.DecoderReferenceNumber)
            || !x.DecoderReferenceNumber.IsNumeric()
            || !(x.DecoderReferenceNumber.Length is 11 or 13)
            || x.DecoderReferenceNumber == "00000000000"
               || x.DecoderReferenceNumber != "0000000000000"
                   && LuhnCheckDigit.LUHN_CHECK_DIGIT.Calculate(x.DecoderReferenceNumber[..^1]) == $"{x.DecoderReferenceNumber[^1]}"
            ));

        Rules.Add(new DelegateRule<MeterTestCommandData>(
            nameof(TestNumber),
            "Invalid Test Number, allowed interval is [0 - 18].",
            x => x.TestNumber <= 18u
            ));
    }
}
