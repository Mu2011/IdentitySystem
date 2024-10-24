using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using IdentitySystem.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace IdentitySystem.Data;

public class IdentitySystemContext(DbContextOptions<IdentitySystemContext> options) :
  IdentityDbContext<AppUser>(options)
{
  public DbSet<Course> Course { get; set; } = default!;

  protected override void OnModelCreating(ModelBuilder builder) 
  {
    builder.ApplyConfiguration(new CourseConfiguration());
    base.OnModelCreating(builder);
  } 
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // base.OnConfiguring(optionsBuilder);
    // Configure DBContext to use SQLite
    optionsBuilder.UseSqlite(@"Data Source=IdentitySystemContext.db");
  }
}
