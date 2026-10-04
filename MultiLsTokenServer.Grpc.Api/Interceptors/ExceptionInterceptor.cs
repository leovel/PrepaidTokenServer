using Grpc.Core;
using Grpc.Core.Interceptors;
using MultiLsTokenServer.Domain.Response;
using MultiLsTokenServer.Domain.Response.Header;
using MultiLsTokenServer.Domain.Response.Payloads;
using System.Diagnostics.CodeAnalysis;

namespace MultiLsTokenServer.Grpc.Api.Interceptors;

using static HsmResponseHeaderFactory;


public class ExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            HsmResponse<Token> result = new()
            {
                Header = ResponseGenerationExceptionHeader(ex.Message),
                Payload = string.Empty
            };

#pragma warning disable IL2091 // Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.
            return MapResponse<TRequest, TResponse>(result);
#pragma warning restore IL2091 // Target generic argument does not satisfy 'DynamicallyAccessedMembersAttribute' in target method or type. The generic parameter of the source method or type does not have matching annotations.
        }
    }

    private static TResponse MapResponse<TRequest, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TResponse>(HsmResponse<Token> result)
    {
        var concreteResponse = Activator.CreateInstance<TResponse>();

        concreteResponse?.GetType().GetProperty(nameof(result.Header))?.SetValue(concreteResponse, result.Header);

        concreteResponse?.GetType().GetProperty(nameof(result.Payload))?.SetValue(concreteResponse, result.Payload);

        return concreteResponse;
    }
}
