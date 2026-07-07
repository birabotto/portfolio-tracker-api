using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Tests.Domain;

public sealed class ProjectTests
{
    [Fact]
    public void Create_ShouldCreateProject_WhenDataIsValid()
    {
        var project = Project.Create(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            "https://portfolio-tracker.example.com",
            ".NET, EF Core, PostgreSQL, Docker");

        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal("Portfolio Tracker API", project.Name);
        Assert.Equal(ProjectStatus.Planning, project.Status);
        Assert.NotEqual(default, project.CreatedAt);
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Project.Create(
                "",
                "A professional API for tracking portfolio projects.",
                "https://github.com/example/portfolio-tracker-api",
                null,
                ".NET, EF Core, PostgreSQL, Docker"));

        Assert.Contains("Project name is required", exception.Message);
    }

    [Fact]
    public void Create_ShouldThrowArgumentException_WhenRepositoryUrlIsInvalid()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            Project.Create(
                "Portfolio Tracker API",
                "A professional API for tracking portfolio projects.",
                "invalid-url",
                null,
                ".NET, EF Core, PostgreSQL, Docker"));

        Assert.Contains("Repository URL must be a valid absolute URL", exception.Message);
    }

    [Fact]
    public void Update_ShouldUpdateProjectData_WhenDataIsValid()
    {
        var project = Project.Create(
            "Old Project",
            "Old description.",
            "https://github.com/example/old-project",
            null,
            ".NET");

        project.Update(
            "Updated Project",
            "Updated description.",
            "https://github.com/example/updated-project",
            "https://updated-project.example.com",
            ".NET, PostgreSQL",
            ProjectStatus.Completed);

        Assert.Equal("Updated Project", project.Name);
        Assert.Equal(ProjectStatus.Completed, project.Status);
        Assert.NotNull(project.UpdatedAt);
    }
}
