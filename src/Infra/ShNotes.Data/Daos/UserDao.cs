using AutoMapper;
using ShNotes.Core.Users;

namespace ShNotes.Data.Daos;

public sealed class UserDaoProfile : Profile
{
    public UserDaoProfile()
    {
        CreateMap<UserDao, User>()
            .ForCtorParam("name", x => x.MapFrom(x => x.Name))
            .ForCtorParam("credential", x => x.MapFrom(x => x));
    }
}

public sealed class CredentialProfile : Profile
{
    public CredentialProfile()
    {
        CreateMap<UserDao, Credential>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.CredentialId))
            .ForMember(d => d.PasswordHash, s => s.MapFrom(x => x.PasswordHash))
            .ForMember(d => d.PasswordSalt, s => s.MapFrom(x => x.PasswordSalt))
            .ForMember(d => d.CreatedAt, s => s.MapFrom(x => x.CredentialCreatedAt));
    }
}

public sealed class UserDao
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CredentialId { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime CredentialCreatedAt { get; set; }
}
