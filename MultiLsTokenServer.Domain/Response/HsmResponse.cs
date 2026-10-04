using MultiLsTokenServer.Domain.Response.Header;

namespace MultiLsTokenServer.Domain.Response;

public class HsmResponse<T>() where T : new()
{
    public HsmResponseHeader Header { get; set; } = HsmResponseHeaderFactory.UnknownCodeHeader;
    public T Payload { get; set; } = new T();
    public bool IsError => Header.IsError;
}
