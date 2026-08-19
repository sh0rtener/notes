namespace ShNotes.WebApi.Common;

public class ApiResponse<T>
{
    public string Message { get; set; } = ApiResponseStatuses.Info;
    public T? Data { get; set; }
    // public Dictionary<string, string[]>? Errors { get; set; }
}
