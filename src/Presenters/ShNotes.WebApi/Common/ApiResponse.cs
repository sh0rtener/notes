namespace ShNotes.WebApi.Common;

public sealed class ApiResponse<T>
{
    public string Message { get; set; } = ApiResponseStatuses.Info;
    public T? Data { get; set; }
    // public Dictionary<string, string[]>? Errors { get; set; }
}
