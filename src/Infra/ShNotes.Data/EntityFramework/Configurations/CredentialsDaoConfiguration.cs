using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShNotes.Data.EntityFramework.Configurations;

public sealed class CredentialsDaoConfiguration : IEntityTypeConfiguration<CredentialDao>
{
    public void Configure(EntityTypeBuilder<CredentialDao> builder)
    {
        builder.ToTable("credentials");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.PasswordHash).HasColumnName("password_hash");
        builder.Property(x => x.PasswordSalt).HasColumnName("password_salt");
        builder.Property(x => x.UserId).HasColumnName("user_id");
    }
}