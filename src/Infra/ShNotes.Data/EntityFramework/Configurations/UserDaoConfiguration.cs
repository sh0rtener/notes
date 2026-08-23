using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShNotes.Data.EntityFramework.Configurations;

public sealed class UserDaoConfiguration : IEntityTypeConfiguration<UserDao>
{
    public void Configure(EntityTypeBuilder<UserDao> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.CredentialId).HasColumnName("credential_id");
        builder.Property(x => x.PasswordSalt).HasColumnName("password_salt");
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash");
        builder.Property(x => x.CredentialCreatedAt).HasColumnName("c_created_at");
    }
}
