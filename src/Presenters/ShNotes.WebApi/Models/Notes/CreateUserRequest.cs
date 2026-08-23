namespace ShNotes.WebApi.Models.Notes;

public sealed class CreateUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
