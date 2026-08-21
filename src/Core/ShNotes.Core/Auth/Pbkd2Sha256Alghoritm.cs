using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ShNotes.Core.Auth;

public sealed record Pbkd2Sha256Pair(string Hash, string Salt);

public sealed class Pbkd2Sha256Alghoritm
{
    public Pbkd2Sha256Pair Crypt(string value, string? base64Salt = null)
    {
        byte[] salt = CreateSalt(128 / 8);
        var hash = KeyDerivation.Pbkdf2(
            password: value,
            salt: string.IsNullOrEmpty(base64Salt) ? salt : Convert.FromBase64String(base64Salt),
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        );

        return new(Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool Validate(string value, Pbkd2Sha256Pair existedPair) =>
        string.Equals(
            Crypt(value, existedPair.Salt).Hash,
            existedPair.Hash,
            StringComparison.OrdinalIgnoreCase
        );

    private byte[] CreateSalt(int size) => RandomNumberGenerator.GetBytes(size);
}
