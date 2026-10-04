using MultiLsTokenServer.Domain.Response.Header;

namespace MultiLsTokenServer.Grpc.Api;

public partial class ApiResponseHeader
{
    public static implicit operator ApiResponseHeader(HsmResponseHeader responseHeader)
    {
        return new ApiResponseHeader
        {
            Code = responseHeader.Code,
            Identifier = responseHeader.Identifier,
            Description = responseHeader.Description,
            IsError = responseHeader.IsError
        };
    }

    public static implicit operator HsmResponseHeader(ApiResponseHeader responseHeader)
    {
        return new HsmResponseHeader
        {
            Code = responseHeader.Code,
            Identifier = responseHeader.Identifier,
            Description = responseHeader.Description
        };
    }
}
