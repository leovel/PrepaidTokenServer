using MultiLsTokenServer.Domain.Response.Payloads;

namespace MultiLsTokenServer.Grpc.Api;

public partial class GetIdentificationPayload
{
    public static implicit operator GetIdentificationPayload(HsmIdentification identification)
    {
        return new GetIdentificationPayload
        {
            ModuleIdentifier = identification.ModuleIdentifier,
            FirmwareIdentifier = identification.FirmwareIdentifier
        };
    }

    public static implicit operator HsmIdentification(GetIdentificationPayload identification)
    {
        return new HsmIdentification
        {
            ModuleIdentifier = identification.ModuleIdentifier,
            FirmwareIdentifier = identification.FirmwareIdentifier
        };
    }
}
