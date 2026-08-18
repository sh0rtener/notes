using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ShNotes.Data.EntityFramework.Contexts;

public sealed class AppDbContext : DbContext
{
    public DbSet<NoteDao> Notes => Set<NoteDao>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
