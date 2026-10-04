using System.Numerics;
using System.Text;
using MultiLsTokenServer.Algorithms.CheckDigit;
using MultiLsTokenServer.Domain.Exceptions;

namespace MultiLsTokenServer.Infrastructure.Tools;

internal static class StandardTrasnferSpecificationTools
{
    private const uint MAX_TRANSFER_AMOUNT = 18_201_624;
    private const string IIN11 = "600727";
    private const string IIN13 = "0000";

    private const uint MAX_CURRENCY_EXPONENT = 0b11111;
    private static readonly BigInteger MAX_CURRENCY_VALUE;
    private static readonly Dictionary<uint, BigInteger> maxValuesByExponentDict = [];

    public const byte CommandFirstByte = 0x00;
    public static byte[] CommandHeaderBytes = [0xED, 0xF1, 0x2B, 0x00, 0xFE, 0xED];
    public const byte CommandLastByte = 0x0D;

    public const string GLER_HEADER = "GL!ER";

    static StandardTrasnferSpecificationTools()
    {
        for (uint i = 1; i <= MAX_CURRENCY_EXPONENT + 1; i++)
        {

            BigInteger max = 0;

            for (int j = 0; j < i; j++)
            {
                max += BigInteger.Pow(10, j);
            }
            maxValuesByExponentDict.Add(i - 1, max * BigInteger.Pow(2, 14) - BigInteger.Pow(10, (int)i - 1));
        };
        MAX_CURRENCY_VALUE = maxValuesByExponentDict[MAX_CURRENCY_EXPONENT];
    }

    public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1, bool fail = true)
    {
        var waitTask = Task.Run(async () =>
        {
            while (condition()) await Task.Delay(frequency);
        });

        if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)) && fail)
            throw new NoHsmResponseException("No Response from the Security Module.");
    }

    public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1, bool fail = true)
    {
        var waitTask = Task.Run(async () =>
        {
            while (!condition()) await Task.Delay(frequency);
        });

        if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)) && fail)
            throw new NoHsmResponseException("No Response from the Security Module.");
    }

    private static ushort BitsSet(byte ch)
    {
        ushort n;
        n = 0;
        while (ch > 0)
        {
            n += (ushort)(ch & 1u);
            ch >>= 1;
        }
        return n;
    }
    public static string CRCof(string message, bool ex = true)
    {
        var len = (uint)message.Length;
        int i;
        ushort crc;
        byte k;
        crc = 0;
        for (i = 0; i < len; i++)
        {
            k = (byte)((byte)message[i] ^ crc);
            crc = (ushort)((ushort)(crc / 256u) ^ (ushort)(k * 128u) ^ (ushort)(k * 64u));
            if ((BitsSet(k) & 1u) != 0)
                crc ^= 0xC001;
        }
        return ex ? $"{crc:X4}" : ((uint)crc).ToBinary(16);
    }

    private static string CalculateCheckDigit(string block)
    {
        return LuhnCheckDigit.LUHN_CHECK_DIGIT.Calculate(block);
    }

    public static string GetMeterPan(string primaryAccountNumber)
    {
        var IIN = primaryAccountNumber.Length == 11 ? IIN11 : IIN13;
        string meterPan = IIN + primaryAccountNumber;
        meterPan += CalculateCheckDigit(meterPan);

        return meterPan;
    }

    public static string ToBinary(this uint number, int bitsLength = 32)
    {
        return NumberToBinary(number, bitsLength);
    }

    public static string NumberToBinary(uint number, int bitsLength = 32)
    {
        string result = Convert.ToString(number, 2).PadLeft(bitsLength, '0');

        return result;
    }

    public static int FromBinaryToInt(this string binary)
    {
        return BinaryToInt(binary);
    }

    public static int BinaryToInt(string binary)
    {
        return Convert.ToInt32(binary, 2);
    }

    private static uint GetExponent(BigInteger amount, BigInteger max_value)
    {
        amount = BigInteger.Min(amount, max_value);

        return maxValuesByExponentDict
            .OrderBy(x => x.Key)
            .FirstOrDefault(x => x.Value >= amount)
            .Key;
    }

    private static BigInteger GetMantissa(ref uint exponent, BigInteger amount, bool negative = false)
    {
        if (exponent == 0)
        {
            return amount;
        }
        else
        {
            BigInteger rhsSum = 0;
            BigInteger lastFactor = 0;
            for (int i = 1; i <= exponent; i++)
            {
                lastFactor = BigInteger.Pow(2, 14) * BigInteger.Pow(10, i - 1);
                rhsSum += lastFactor;
            }

            if (negative && rhsSum > amount)
            {
                rhsSum -= lastFactor;
                exponent--;
            }

            var (Quotient, Remainder) = BigInteger.DivRem(amount - rhsSum, BigInteger.Pow(10, (int)exponent));

            return negative || Remainder <= BigInteger.Zero ? Quotient : Quotient + 1;
        }
    }

    private static BigInteger GetAbsAmount(uint exponent, uint mantisa)
    {
        var t0 = BigInteger.Pow(10, (int)exponent) * mantisa;
        if (exponent > 0)
        {
            for (int i = 1; i <= exponent; i++)
            {
                t0 += BigInteger.Pow(2, 14) * BigInteger.Pow(10, i - 1);
            }
        }

        return t0;
    }

    public static string GetAmountBlock(double transferAmount, uint tClass, uint tSubClass)
    {
        if (tClass == 0 && tSubClass < 4 || tClass == 2 && tSubClass is 0 or 6 or 7)
        {
            var truncatedAmount = tClass == 0 || tSubClass is 7 ? (long)Math.Ceiling(transferAmount * 10.0)
                : (long)Math.Ceiling(transferAmount);

            if (!int.TryParse($"{truncatedAmount}", out int complementedAmount)
                || complementedAmount < 0
                || complementedAmount > MAX_TRANSFER_AMOUNT)
            {
                throw new InvalidTransferAmmountException(
                    nameof(transferAmount),
                    transferAmount,
                    $"Transfer Ammount out of range");
            }

            uint exponent = GetExponent(complementedAmount, MAX_TRANSFER_AMOUNT);
            uint mantissa = (uint)GetMantissa(ref exponent, complementedAmount);

            return $"{(exponent.ToBinary(2) + mantissa.ToBinary(14)).FromBinaryToInt():X4}";

        }
        else if (tClass == 0 && tSubClass is >= 4 and <= 7)
        {
            var sign = transferAmount < 0 ? 1 : 0;
            var complementedAmount = BigInteger.Abs((BigInteger)Math.Ceiling(transferAmount * 100_000.0));

            if (complementedAmount > MAX_CURRENCY_VALUE)
            {
                throw new InvalidTransferAmmountException(
                    nameof(transferAmount),
                    transferAmount,
                    $"Transfer Ammount out of range");
            }

            uint exponent = GetExponent(complementedAmount, MAX_CURRENCY_VALUE);
            uint mantissa = (uint)GetMantissa(ref exponent, complementedAmount, sign == 1);

            uint e10 = exponent << 30 >> 30;
            string eS432 = $"{sign}{(exponent >> 2).ToBinary(3)}";

            return $"{eS432.FromBinaryToInt():X2}{(e10.ToBinary(2) + mantissa.ToBinary(14)).FromBinaryToInt():X4}";
        }
        else
        {
            if (!int.TryParse($"{(long)Math.Ceiling(transferAmount)}", out int complementedAmount)
                || complementedAmount < 0
                || complementedAmount > 65535)
            {
                throw new InvalidTransferAmmountException(
                    nameof(transferAmount),
                    transferAmount,
                    $"Transfer Ammount out of range");
            }

            return $"{complementedAmount:X4}";
        }
    }

    public static double GetTransferAmount(string amountBlock, uint tClass, uint tSubClass)
    {
        if (tClass == 0 && tSubClass is >= 4 and <= 7)
        {
            //uint numericBlockS432 = Convert.ToUInt32(amountBlock[0..2], 16);
            //uint e432 = numericBlockS432 & ((1 << 3) - 1);
            //uint sign = numericBlockS432 >> 3;

            //uint numericBlock10m = Convert.ToUInt32(amountBlock[2..], 16);

            //uint mantisa = numericBlock10m & ((1 << 14) - 1);
            //uint e10 = numericBlock10m >> 14;

            //var exponent = (e432 << 2) + e10;

            //var absAmount = (double)GetAbsAmount(exponent, mantisa) / 100_000.0;

            //return sign == 1u ? -absAmount : absAmount;

            return 0.0;
        }
        else
        {
            uint numericBlock = Convert.ToUInt32(amountBlock, 16);

            if (tClass == 0 || tClass == 2 && tSubClass is 0 or 6 or 7)
            {

                uint mantisa = numericBlock & (1 << 14) - 1;
                uint exponent = numericBlock >> 14;

                var absAmount = GetAbsAmount(exponent, mantisa);

                var ammount = tClass == 0 || tSubClass is 7 ? (double)absAmount / 10.0
                    : (double)absAmount;

                return ammount;

            }
            else
            {
                return numericBlock;
            }
        }
    }

    public static string GetTIDBlock(DateTime issueDateTime, uint base_year, bool isTidCommand)
    {
        var base_date = new DateTime((int)base_year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        if (base_date > issueDateTime)
        {
            if (isTidCommand)
            {
                throw new TidCalculationException(
                nameof(issueDateTime),
                issueDateTime,
                $"Issue date before base year [{base_year}]");
            }
            else
                issueDateTime = new DateTime(base_date.Year, issueDateTime.Month, issueDateTime.Day, issueDateTime.Hour, issueDateTime.Minute, 0, DateTimeKind.Utc);
        }



        return $"{(uint)(issueDateTime - base_date).TotalMinutes:X6}";
    }

    public static DateTime GetIssueDateTime(string tidBlock, uint base_year)
    {
        var base_date = new DateTime((int)base_year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        var tid = Convert.ToUInt32(tidBlock, 16);

        return base_date.AddMinutes(tid);
    }

    public static string FormatToken(string unformatedToken)
    {
        if (string.IsNullOrWhiteSpace(unformatedToken) || unformatedToken.Length != 20)
            return string.Empty;

        StringBuilder formatedData = new();
        int count = 1;
        foreach (char c in unformatedToken)
        {
            formatedData.Append(c);

            if (count < 20 && count % 4 == 0)
                formatedData.Append(' ');
            count++;
        }

        return formatedData.ToString();
    }

    public static List<string> FormatTokens(string unformatedTokens)
    {
        List<string> result = [];
        if (!string.IsNullOrWhiteSpace(unformatedTokens) && unformatedTokens.Length % 20 == 0)
        {
            var n = unformatedTokens.Length / 20;

            for (int i = 0; i < n; i++)
            {
                result.Add(FormatToken(unformatedTokens.Substring(i * 20, 20)));
            }
        }

        return result;
    }

    #region TEST TOKEN GENERATION
    //public static string GetTestToken(int manufacturerCodeDigits, uint testNumber)
    //{
    //    return manufacturerCodeDigits switch
    //    {
    //        2 => GetTestToken(testNumber, 36),
    //        4 => GetTestToken(testNumber, 28),
    //        _ => throw new InvalidDecoderReferenceNumberException($"Invalid Decoder Reference Number Format.", nameof(manufacturerCodeDigits))
    //    };
    //}

    //private static string GetTestToken(uint testNumber, int numControlBits)
    //{
    //    if (testNumber > numControlBits)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(testNumber), testNumber, $"Invalid Test Number.");
    //    }

    //    var tokenClass = "01";
    //    var subClass = numControlBits == 36 ? "0000" : "0001";
    //    var mfrCode = string.Empty.PadLeft(44 - numControlBits, '0');

    //    string control = testNumber == 0 ? string.Empty.PadLeft(numControlBits, '1') :
    //        $"{((uint)Math.Pow(2, testNumber - 1)).ToBinary(numControlBits)}";


    //    string token48 = $"{subClass}{control}{mfrCode}";

    //    var token50Ex = $"{Convert.ToInt64($"{tokenClass}{token48}", 2):X}";
    //    var token50ExPad56 = token50Ex.PadLeft(14, '0');

    //    var crc = CRCof(token50ExPad56, false);

    //    var encriptedToken = $"{token48}{crc}";

    //    var fullToken = InsertAndTranspositionClassBits(encriptedToken, tokenClass);

    //    BigInteger token = 0;

    //    for (int i = 0; i < 66; i++)
    //    {
    //        token += fullToken[i] == '0' ? BigInteger.Zero : BigInteger.Pow(2, 65 - i);
    //    }

    //    return FormatToken($"{token:D20}");
    //}

    //private static string InsertAndTranspositionClassBits(string encryptedTokenBlock, string tokenClass)
    //{
    //    string withClassBits = tokenClass + encryptedTokenBlock;
    //    char[] tokenClassBits = tokenClass.ToCharArray();
    //    char[] tokenBlockBits = withClassBits.ToCharArray();
    //    tokenBlockBits[withClassBits.Length - 1 - 65] = tokenBlockBits[withClassBits.Length - 1 - 28];
    //    tokenBlockBits[withClassBits.Length - 1 - 64] = tokenBlockBits[withClassBits.Length - 1 - 27];
    //    tokenBlockBits[withClassBits.Length - 1 - 28] = tokenClassBits[0];
    //    tokenBlockBits[withClassBits.Length - 1 - 27] = tokenClassBits[1];

    //    return string.Join("", tokenBlockBits);
    //}
    #endregion
}
