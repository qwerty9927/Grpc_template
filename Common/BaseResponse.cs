using Grpc.Core;

namespace Grpc_template.Common;

public class BaseResponse
{
    public string Message { get; set; } = string.Empty;

    public int Code { get; set; }

    public object Data { get; set; }
}

public class BaseResponse<T> : BaseResponse
{
    public BaseResponse() { }

    public BaseResponse(T? data, string message, int code)
    {
        Message = message;
        Code = code;
        Data = data;
    }
}

public class SuccessResponse<T>(
    T? data,
    string message = nameof(StatusCode.OK),
    StatusCode code = StatusCode.OK) :
    BaseResponse<T>(data, message, (int)code) { }
