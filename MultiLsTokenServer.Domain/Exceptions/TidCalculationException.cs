namespace MultiLsTokenServer.Domain.Exceptions;

public class TidCalculationException(string? paramName, object? actualValue, string? message)
    : ParameterOutOfRangeException(paramName, actualValue, message)
{ }