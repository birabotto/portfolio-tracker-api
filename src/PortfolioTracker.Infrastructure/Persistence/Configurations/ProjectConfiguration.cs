using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Infrastructure.Persistence.Configurations;

public sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(project => project.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(project => project.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(project => project.RepositoryUrl)
            .HasColumnName("repository_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(project => project.DemoUrl)
            .HasColumnName("demo_url")
            .HasMaxLength(500);

        builder.Property(project => project.TechStack)
            .HasColumnName("tech_stack")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(project => project.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(project => project.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(project => project.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(project => project.Name)
            .IsUnique();
    }
}
