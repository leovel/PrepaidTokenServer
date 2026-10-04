using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class ManagementTokenRequest
{
    public static implicit operator TokenCommandData(ManagementTokenRequest request)
    {
        return new TokenCommandData(
            request.KeyRegister,
            request.DecoderReferenceNumber,
            request.TariffIndex,
            request.EncryptionAlgorithm,
            request.TokenCarrierType,
            request.TransferAmmount,
            request.IssueDateTime);
    }

    public static implicit operator ManagementTokenRequest(TokenCommandData commandData)
    {
        return new ManagementTokenRequest
        {
            KeyRegister = commandData.KeyRegister,
            DecoderReferenceNumber = commandData.DecoderReferenceNumber,
            TariffIndex = commandData.TariffIndex,
            EncryptionAlgorithm = commandData.EncryptionAlgorithm,
            TokenCarrierType = commandData.TokenCarrierType,
            TransferAmmount = commandData.TransferAmmount,
            IssueDateTime = commandData.IssueDateTime
        };
    }
}
