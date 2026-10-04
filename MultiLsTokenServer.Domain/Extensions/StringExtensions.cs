using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiLsTokenServer.Domain;

public static partial class StringExtensions
{
    public static bool MyMatch(this string target, string value, bool quitSpacing = false)
    {
        if (string.IsNullOrWhiteSpace(target))
            return string.IsNullOrWhiteSpace(value);

        if (string.IsNullOrWhiteSpace(value))
            return true;

        return target.RemoveDiacritics(quitSpacing)
            .Contains(value.RemoveDiacritics(quitSpacing), StringComparison.CurrentCultureIgnoreCase);
    }

    public static string RemoveDiacritics(this string text, bool quitSpacing = false)
    {
        return string.Concat(
            text.Normalize(NormalizationForm.FormD)
            .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark && (!quitSpacing || CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.SpaceSeparator))
          ).Normalize(NormalizationForm.FormC);
    }

    public static Regex PhoneNumberRegex { get; } = AngolanPhoneNumberRegex();

    public static bool IsAngolanPhoneNumber(this string target, bool emptyValid = false)
    {
        if (string.IsNullOrWhiteSpace(target))
            return emptyValid;

        return PhoneNumberRegex.IsMatch(target);
    }

    public static Regex EmailRegex { get; } = GeneralEmailRegex();

    public static bool IsEmail(this string target, bool emptyValid = false)
    {
        if (string.IsNullOrWhiteSpace(target))
            return emptyValid;

        return EmailRegex.IsMatch(target);
    }

    public static Regex NumericRegex { get; } = GeneralNumericRegex();
    public static bool IsNumeric(this string target, bool emptyValid = false)
    {
        if (string.IsNullOrWhiteSpace(target))
            return emptyValid;

        return NumericRegex.IsMatch(target);
    }

    static readonly uint minYear = (uint)DateTime.MinValue.Year;
    static readonly uint maxYear = (uint)DateTime.MaxValue.Year;

    public static bool TryConvertToDateTime(this string text, out DateTime dateTime)
    {
        if (text.Length == 14
            && int.TryParse(text[..4], out int year) && year >= minYear && year <= maxYear
            && int.TryParse(text[4..6], out int month) && month >= 1 && month <= 12
            && int.TryParse(text[6..8], out int day) && day >= 1 && day <= DateTime.DaysInMonth(year, month)
            && int.TryParse(text[8..10], out int hour) && hour >= 0 && hour <= 23
            && int.TryParse(text[10..12], out int minute) && minute >= 0 && minute <= 59
            && int.TryParse(text[12..14], out int second) && second >= 0 && second <= 59)
        {
            dateTime = new DateTime
            (
                year,
                month,
                day,
                hour,
                minute,
                second
            );

            return true;
        }
        else
        {
            dateTime = default;
            return false;
        }
    }

    [GeneratedRegex(@"^(([\+]|([0]{2}))244([-]|[ ])?)?(([9][1-9])|[2]{2})[0-9]{7}$", RegexOptions.Singleline)]
    private static partial Regex AngolanPhoneNumberRegex();

    [GeneratedRegex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Singleline | RegexOptions.CultureInvariant)]
    private static partial Regex GeneralEmailRegex();

    [GeneratedRegex(@"^[0-9]+$", RegexOptions.Singleline)]
    private static partial Regex GeneralNumericRegex();
}
