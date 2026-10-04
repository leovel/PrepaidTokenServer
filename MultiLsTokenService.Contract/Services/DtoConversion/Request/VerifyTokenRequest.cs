using MultiLsTokenServer.Domain.Request;

namespace MultiLsTokenServer.Grpc.Api;

public partial class VerifyTokenRequest
{
    public static implicit operator TokenVerificationCommandData(VerifyTokenRequest request)
    {
        return new TokenVerificationCommandData(
            request.KeyRegister,
            request.DecoderReferenceNumber,
            request.TariffIndex,
            request.EncryptionAlgorithm,
            request.TokenDec);
    }

    public static implicit operator VerifyTokenRequest(TokenVerificationCommandData commandData)
    {
        return new VerifyTokenRequest
        {
            KeyRegister = commandData.KeyRegister,
            DecoderReferenceNumber = commandData.DecoderReferenceNumber,
            TariffIndex = commandData.TariffIndex,
            EncryptionAlgorithm = commandData.EncryptionAlgorithm,
            TokenDec = commandData.TokenDec
        };
    }
}
