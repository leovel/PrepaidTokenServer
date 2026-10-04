namespace MultiLsTokenServer.Algorithms.CheckDigit;

public abstract class ModulusCheckDigit : ICheckDigit
{
    private readonly int modulus;


    public ModulusCheckDigit(int modulus)
    {
        this.modulus = modulus;
    }


    public int getModulus()
    {
        return modulus;
    }


    public bool IsValid(string code)
    {
        if (code == null || code.Length == 0)
        {
            return false;
        }
        try
        {
            int modulusResult = calculateModulus(code, true);
            return modulusResult == 0;
        }
        catch (CheckDigitException)
        {
            return false;
        }
    }


    public string Calculate(string code)
    {
        if (code == null || code.Length == 0)
        {
            throw new CheckDigitException("Code is missing");
        }
        int modulusResult = calculateModulus(code, false);
        int charValue = (modulus - modulusResult) % modulus;
        return toCheckDigit(charValue);
    }


    protected int calculateModulus(string code, bool includesCheckDigit)
    {
        int total = 0;
        for (int i = 0; i < code.Length; i++)
        {
            int lth = code.Length + (includesCheckDigit ? 0 : 1);
            int leftPos = i + 1;
            int rightPos = lth - i;
            int charValue = toInt(code[i], leftPos, rightPos);
            total += weightedValue(charValue, leftPos, rightPos);
        }
        if (total == 0)
        {
            throw new CheckDigitException("Invalid code, sum is zero");
        }
        return total % modulus;
    }


    protected abstract int weightedValue(int charValue, int leftPos, int rightPos);



    protected int toInt(char character, int leftPos, int rightPos)
    {
        if (char.IsDigit(character))
        {
            return int.Parse($"{character}");
        }
        throw new CheckDigitException("Invalid Character[" +
                leftPos + "] = '" + character + "'");
    }


    protected string toCheckDigit(int charValue)
    {
        if (charValue >= 0 && charValue <= 9)
        { // CHECKSTYLE IGNORE MagicNumber
            return $"{charValue}";
        }
        throw new CheckDigitException("Invalid Check Digit Value =" +
                +charValue);
    }


    public static int sumDigits(int number)
    {
        int total = 0;
        int todo = number;
        while (todo > 0)
        {
            total += todo % 10; // CHECKSTYLE IGNORE MagicNumber
            todo = todo / 10; // CHECKSTYLE IGNORE MagicNumber
        }
        return total;
    }

}
