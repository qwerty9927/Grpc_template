using Grpc_template.Common;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Grpc_template.Interceptors;

public class CallProcessingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (BaseException ex)
        {
            throw HandleException(context.GetHttpContext(), ex);
        }
        catch (Exception ex)
        {
            throw HandleException(context.GetHttpContext(), ex);
        }
    }

    private RpcException HandleException(HttpContext context, object exception)
    {
        Status? status = default;
        if (exception is BaseException baseException)
        {
            status = new Status((StatusCode)baseException.Response.Code, baseException.Response.Message);
        }

        return new RpcException(status ?? new Status(StatusCode.Internal, "Internal Server Error"));
    }
}