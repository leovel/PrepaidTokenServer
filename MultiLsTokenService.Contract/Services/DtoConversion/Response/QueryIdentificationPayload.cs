using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class QueryIdentificationPayload
{
    public static implicit operator QueryIdentificationPayload(HsmQueryIdentification queryIdentification)
    {
        return new QueryIdentificationPayload
        {
            PublicKeyIdentifier = queryIdentification.PublicKeyIdentifier,
            HardwareIdentifier = queryIdentification.HardwareIdentifier,
            FirmwareIdentifier = queryIdentification.FirmwareIdentifier,
            FirmwareHash = queryIdentification.FirmwareHash
        };
    }

    public static implicit operator HsmQueryIdentification(QueryIdentificationPayload queryIdentification)
    {
        return new HsmQueryIdentification
        {
            PublicKeyIdentifier = queryIdentification.PublicKeyIdentifier,
            HardwareIdentifier = queryIdentification.HardwareIdentifier,
            FirmwareIdentifier = queryIdentification.FirmwareIdentifier,
            FirmwareHash = queryIdentification.FirmwareHash
        };
    }
}
