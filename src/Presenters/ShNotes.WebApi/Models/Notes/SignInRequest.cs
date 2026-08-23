namespace ShNotes.WebApi.Models.Notes;

public sealed class SignInRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
