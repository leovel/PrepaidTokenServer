namespace MultiLsTokenServer.Algorithms.CheckDigit;

public sealed class LuhnCheckDigit : ModulusCheckDigit
{
    public static readonly ICheckDigit LUHN_CHECK_DIGIT = new LuhnCheckDigit();

    /** weighting given to digits depending on their right position */
    private static readonly int[] POSITION_WEIGHT = new int[] { 2, 1 };

    /**
     * Construct a modulus 10 Luhn Check Digit routine.
     */
    private LuhnCheckDigit() : base(10)
    {
    }


    protected override int weightedValue(int charValue, int leftPos, int rightPos)
    {
        int weight = POSITION_WEIGHT[rightPos % 2]; // CHECKSTYLE IGNORE MagicNumber
        int weightedValue = charValue * weight;
        return weightedValue > 9 ? weightedValue - 9 : weightedValue; // CHECKSTYLE IGNORE MagicNumber
    }
}
