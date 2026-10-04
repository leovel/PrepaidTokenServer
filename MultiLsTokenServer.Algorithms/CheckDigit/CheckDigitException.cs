namespace MultiLsTokenServer.Algorithms.CheckDigit;

public class CheckDigitException : Exception
{
    public CheckDigitException()
    {
    }

    public CheckDigitException(string? message) : base(message)
    {
    }

    public CheckDigitException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
