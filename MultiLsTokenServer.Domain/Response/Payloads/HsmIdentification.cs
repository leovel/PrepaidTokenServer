namespace MultiLsTokenServer.Domain.Response.Payloads;

public record HsmIdentification(string ModuleIdentifier, string FirmwareIdentifier)
{
    public HsmIdentification() : this(string.Empty, string.Empty) { }
}
