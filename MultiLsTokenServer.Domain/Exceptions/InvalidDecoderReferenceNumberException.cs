namespace MultiLsTokenServer.Domain.Exceptions;

public class InvalidDecoderReferenceNumberException(string? message, string? paramName)
    : ArgumentException(message, paramName)
{ }
