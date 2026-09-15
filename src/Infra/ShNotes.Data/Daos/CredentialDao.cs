namespace ShNotes.Data.Daos;

public sealed class CredentialDao
{
    public int Id { get; set; }

    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? UserId { get; set; }
}
