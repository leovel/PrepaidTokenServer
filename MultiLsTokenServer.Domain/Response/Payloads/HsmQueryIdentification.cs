namespace MultiLsTokenServer.Domain.Response.Payloads;

public record HsmQueryIdentification(string PublicKeyIdentifier, string HardwareIdentifier, string FirmwareIdentifier, string FirmwareHash)
{
    public HsmQueryIdentification() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
}
