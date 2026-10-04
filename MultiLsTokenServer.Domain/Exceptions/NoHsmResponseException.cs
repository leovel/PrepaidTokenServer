namespace MultiLsTokenServer.Domain.Exceptions;

public class NoHsmResponseException(string? message) : TimeoutException(message) { }
