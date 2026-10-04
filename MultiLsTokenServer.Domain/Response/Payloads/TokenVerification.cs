namespace MultiLsTokenServer.Domain.Response.Payloads;

public record TokenVerification(bool Valid, uint TokenClass, uint SubClass, double TransferAmmount, DateTime IssueDateTime)
{
    public TokenVerification() : this(false, 0, 0, 0.0, default) { }
}
