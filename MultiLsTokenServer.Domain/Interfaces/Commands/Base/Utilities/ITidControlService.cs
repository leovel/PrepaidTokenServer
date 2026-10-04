namespace MultiLsTokenServer.Domain.Interfaces.Commands.Base.Utilities
{
    public interface ITidControlService
    {
        DateTime GetTID(string decoderReferenceNumber, DateTime issueDateTime,uint keyRegister, bool isTidCommand);
    }
}