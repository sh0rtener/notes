namespace ShNotes.WebApi.Models.Notes;

/// <summary>
/// Запрос на обновление JWT токена
/// </summary>
public sealed class RefreshTokenRequest
{
    /// <summary>
    /// Refresh токен
    /// </summary>
    public required string RefreshToken { get; set; }
}

