using ShNotes.WebApi.Common;

namespace ShNotes.WebApi.Swagger;

public sealed class BadRequestApiResponse
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>bad request</example>
    public string Message { get; set; } = ApiResponseStatuses.ClientError;
    /// <summary>
    /// 
    /// </summary>
    /// <example>some client error</example>
    public string? Data { get; set; } = null!;
}
