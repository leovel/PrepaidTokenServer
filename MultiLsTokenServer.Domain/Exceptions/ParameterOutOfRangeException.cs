namespace MultiLsTokenServer.Domain.Exceptions;

public abstract class ParameterOutOfRangeException(string? paramName, object? actualValue, string? message)
: ArgumentOutOfRangeException(paramName, actualValue, message)
{ }
