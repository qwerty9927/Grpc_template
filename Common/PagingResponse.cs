namespace Grpc_template.Common;

public class PagingResponse { }

public class PagingResponse<T> : PagingResponse
{
    public new List<T> Records { get; set; } = [];

    public int TotalRecord { get; set; }
}