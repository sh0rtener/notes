namespace ShNotes.WebApi.Jwt;

public class JwtConfiguration
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public long AccessTokenExpires { get; set; }
    public long RefreshTokenExpires { get; set; }
}
