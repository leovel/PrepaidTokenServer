using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class KeyChangeTokenRequest
{
    public static implicit operator KeyChangeCommandData(KeyChangeTokenRequest request)
    {
        return new KeyChangeCommandData(
            request.KeyRegisterOld,
            request.KeyRegisterNew,
            request.DecoderReferenceNumber,
            request.TariffIndexOld,
            request.EncryptionAlgorithm,
            request.TokenCarrierType,
            request.TariffIndexNew,
            request.NumTokens);
    }

    public static implicit operator KeyChangeTokenRequest(KeyChangeCommandData commandData)
    {
        return new KeyChangeTokenRequest
        {
            KeyRegisterOld = commandData.KeyRegisterOld,
            KeyRegisterNew = commandData.KeyRegister,
            DecoderReferenceNumber = commandData.DecoderReferenceNumber,
            TariffIndexOld = commandData.TariffIndexOld,
            EncryptionAlgorithm = commandData.EncryptionAlgorithm,
            TokenCarrierType = commandData.TokenCarrierType,
            TariffIndexNew = commandData.TariffIndexNew,
            NumTokens = commandData.NumTokens
        };
    }
}
