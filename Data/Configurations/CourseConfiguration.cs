using IdentitySystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentitySystem.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
  public void Configure(EntityTypeBuilder<Course> builder)
  {
    builder.HasKey(c => c.CourseID);

    builder.Property(c => c.CourseName)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(c => c.Price)
      .IsRequired();
  }
}