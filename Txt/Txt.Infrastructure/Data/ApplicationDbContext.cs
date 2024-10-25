using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Txt.Domain.Entities;

namespace Txt.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext() { }

    public DbSet<Note> Notes { get; set; }

    public DbSet<NoteLine> NoteLines { get; set; }

    public DbSet<Folder> Folders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Note>().ToTable("Notes");
        modelBuilder.Entity<User>().ToTable("Users");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connString = Environment.GetEnvironmentVariable("defaultConnection")
            ?? throw new Exception("Connection string not found in environment.");

        optionsBuilder.UseSqlServer(connString);
    }
}

