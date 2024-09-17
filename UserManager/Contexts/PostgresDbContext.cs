using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManager.Entities;

namespace UserManager.Contexts;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : IdentityDbContext<User, Role, int>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}