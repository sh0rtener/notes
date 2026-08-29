using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShNotes.WebApi.Models.Notes;

namespace ShNotes.WebApi.Jwt;

public class JwtService
{
    public readonly JwtConfiguration Configuration;

    public JwtService(IOptions<JwtConfiguration> configuration)
    {
        Configuration = configuration.Value;
    }

    public SignInResponse GenerateTokenResponse(int userId, string userName)
    {
        var accessExpiresAt = DateTime.Now.AddMinutes(Configuration.AccessTokenExpires);
        var refreshExpiresAt = DateTime.Now.AddMinutes(Configuration.RefreshTokenExpires);

        var accessClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        var refreshClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Thumbprint, userName),
        };

        var accessToken = CreateToken(accessClaims, accessExpiresAt);
        var refreshToken = CreateToken(refreshClaims, refreshExpiresAt);

        return new SignInResponse
        {
            AccessToken = WriteToken(accessToken),
            RefreshToken = WriteToken(refreshToken),
            ExpiresAt = accessExpiresAt,
            RefreshExpiresAt = refreshExpiresAt,
        };
    }

    public JwtSecurityToken CreateToken(List<Claim> claims, DateTime expires)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(Configuration.Key);
        return new JwtSecurityToken(
            issuer: Configuration.Issuer,
            audience: Configuration.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(tokenBytes),
                SecurityAlgorithms.HmacSha256
            )
        );
    }

    public ClaimsPrincipal GetClaimsPrincipal(string token)
    {
        if (string.IsNullOrEmpty(token))
            throw new InvalidDataException("Token is empty");

        byte[] key = Encoding.UTF8.GetBytes(Configuration.Key);

        try
        {
            ClaimsPrincipal principal = ValidateToken(token, key, out SecurityToken securityToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)securityToken;

            if (
                securityToken is null
                || !jwtToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
                throw new SecurityTokenException("Токен неверный");

            return principal;
        }
        catch
        {
            throw new InvalidDataException("Invalid token");
        }
    }

    public string WriteToken(JwtSecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);

    private ClaimsPrincipal ValidateToken(
        string token,
        byte[] key,
        out SecurityToken outSecurityToken
    )
    {
        ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            },
            out SecurityToken securityToken
        );
        outSecurityToken = securityToken;

        return principal;
    }
}
