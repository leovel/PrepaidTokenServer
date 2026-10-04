using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class TransferCreditTokenRequest
{
    public static implicit operator TokenCommandData(TransferCreditTokenRequest request)
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

    public static implicit operator TransferCreditTokenRequest(TokenCommandData commandData)
    {
        return new TransferCreditTokenRequest
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
