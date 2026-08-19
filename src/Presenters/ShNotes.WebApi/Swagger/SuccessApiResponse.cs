using ShNotes.WebApi.Common;

namespace ShNotes.WebApi.Swagger;

public sealed class SuccessApiResponse<T> : ApiResponse<T>
{
    public SuccessApiResponse()
    {
        Message = ApiResponseStatuses.Success;
    }
}
