namespace MultiLsTokenServer.Domain.Exceptions;

public class KeyValidationException(string? message) : ApplicationException(message)
{
}
