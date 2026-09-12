using Newtonsoft.Json;

namespace ShNotes.WebApi.Common;

public class ApiResponse<T>
{
    [JsonProperty("message")]
    public string Message { get; set; } = ApiResponseStatuses.Info;
    
    [JsonProperty("data")]
    public T? Data { get; set; }
    // public Dictionary<string, string[]>? Errors { get; set; }
}
