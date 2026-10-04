using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class VerifyTokenPayload
{
    public static implicit operator VerifyTokenPayload(TokenVerification tokenVerification)
    {
        return new VerifyTokenPayload
        {
            Valid = tokenVerification.Valid,
            TokenClass = tokenVerification.TokenClass,
            SubClass = tokenVerification.SubClass,
            TransferAmmount = tokenVerification.TransferAmmount,
            IssueDateTime = tokenVerification.IssueDateTime
        };
    }

    public static implicit operator TokenVerification(VerifyTokenPayload verifyTokenPayload)
    {
        return new TokenVerification
        {
            Valid = verifyTokenPayload.Valid,
            TokenClass = verifyTokenPayload.TokenClass,
            SubClass = verifyTokenPayload.SubClass,
            TransferAmmount = verifyTokenPayload.TransferAmmount,
            IssueDateTime = verifyTokenPayload.IssueDateTime
        };
    }
}
