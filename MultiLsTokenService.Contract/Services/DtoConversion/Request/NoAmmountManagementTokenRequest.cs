using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class NoAmmountManagementTokenRequest
{
    public static implicit operator TokenCommandData(NoAmmountManagementTokenRequest request)
    {
        return new TokenCommandData(
            request.KeyRegister,
            request.DecoderReferenceNumber,
            request.TariffIndex,
            request.EncryptionAlgorithm,
            request.TokenCarrierType,
            0.0,
            request.IssueDateTime);
    }

    public static implicit operator NoAmmountManagementTokenRequest(TokenCommandData commandData)
    {
        return new NoAmmountManagementTokenRequest
        {
            KeyRegister = commandData.KeyRegister,
            DecoderReferenceNumber = commandData.DecoderReferenceNumber,
            TariffIndex = commandData.TariffIndex,
            EncryptionAlgorithm = commandData.EncryptionAlgorithm,
            TokenCarrierType = commandData.TokenCarrierType,
            IssueDateTime = commandData.IssueDateTime
        };
    }
}
