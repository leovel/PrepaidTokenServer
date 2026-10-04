namespace MultiLsTokenServer.Domain.Request.Base;

public interface IKeyRegisterCommandData
{
    uint KeyRegister { get; }
    string DecoderReferenceNumber { get; }
    DateTime IssueDateTime { get; }
    bool RequireValidKey { get; }
    bool IsTidCommand { get; }
}
