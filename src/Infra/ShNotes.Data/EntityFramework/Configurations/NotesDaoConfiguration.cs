using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShNotes.Data.Daos;

namespace ShNotes.Data.EntityFramework.Configurations;

public sealed class NotesDaoConfiguration : IEntityTypeConfiguration<NoteDao>
{
    public void Configure(EntityTypeBuilder<NoteDao> builder)
    {
        builder.ToTable("notes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.Status).HasColumnName("status");
        builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }
}
