namespace MultiLsTokenServer.Domain.Exceptions;

public class InvalidTransferAmmountException(string? paramName, object? actualValue, string? message)
    : ParameterOutOfRangeException(paramName, actualValue, message)
{ }
