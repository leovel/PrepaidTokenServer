namespace MultiLsTokenServer.Grpc.Api;

public partial class CheckSecurityModulePayload
{
    public static implicit operator CheckSecurityModulePayload(bool connected) => new() { Connected = connected };
    public static implicit operator bool(CheckSecurityModulePayload payload) => payload.Connected;
}
