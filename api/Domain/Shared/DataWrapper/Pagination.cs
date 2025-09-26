namespace api.Domain.Shared.DataWrapper;

public interface IPaginationResponse<TResult>
{
    IList<TResult> Items { get; set; }
    int PageNumber { get; set; }
    int PageSize { get; set; }
    int TotalItems { get; set; }
}

public class BasePaginationResponse<TResult> : IPaginationResponse<TResult>
{
    public required IList<TResult> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
}