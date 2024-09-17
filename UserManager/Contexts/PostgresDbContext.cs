using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManager.Entities;

namespace UserManager.Contexts;

public class PostgresDbContext : IdentityDbContext<User, Role, int>
{
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}