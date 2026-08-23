using AutoMapper;
using ShNotes.Core.Users;

namespace ShNotes.UseCases.Users;

public sealed class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateMap<User, UserDto>();
    }
}

public sealed class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
