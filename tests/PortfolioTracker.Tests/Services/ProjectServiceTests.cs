using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Services;
using PortfolioTracker.Domain.Enums;
using PortfolioTracker.Tests.Fakes;

namespace PortfolioTracker.Tests.Services;

public sealed class ProjectServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateProject_WhenRequestIsValid()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var request = new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker");

        var project = await service.CreateAsync(request);

        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal("Portfolio Tracker API", project.Name);
        Assert.Equal(ProjectStatus.Planning, project.Status);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenProjectNameAlreadyExists()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var request = new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker");

        await service.CreateAsync(request);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateAsync(request));

        Assert.Contains("A project with this name already exists", exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProjects()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        await service.CreateAsync(new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker"));

        await service.CreateAsync(new CreateProjectRequest(
            "Task Manager API",
            "A simple task management API.",
            "https://github.com/example/task-manager-api",
            null,
            ".NET, PostgreSQL"));

        var projects = await service.GetAllAsync();

        Assert.Equal(2, projects.Count);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnFilteredPagedProjects()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        await service.CreateAsync(new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker"));

        await service.CreateAsync(new CreateProjectRequest(
            "Task Manager API",
            "A simple task management API.",
            "https://github.com/example/task-manager-api",
            null,
            ".NET, PostgreSQL"));

        var result = await service.GetPagedAsync(new ProjectQueryParameters(
            "portfolio",
            ProjectStatus.Planning,
            1,
            10));

        Assert.Single(result.Items);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal("Portfolio Tracker API", result.Items[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProject_WhenProjectExists()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var createdProject = await service.CreateAsync(new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker"));

        var project = await service.GetByIdAsync(createdProject.Id);

        Assert.NotNull(project);
        Assert.Equal(createdProject.Id, project.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProjectDoesNotExist()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var request = new UpdateProjectRequest(
            "Updated Project",
            "Updated description.",
            "https://github.com/example/updated-project",
            null,
            ".NET, PostgreSQL",
            ProjectStatus.Completed);

        var result = await service.UpdateAsync(Guid.NewGuid(), request);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenProjectExists()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var createdProject = await service.CreateAsync(new CreateProjectRequest(
            "Portfolio Tracker API",
            "A professional API for tracking portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker"));

        var deleted = await service.DeleteAsync(createdProject.Id);

        Assert.True(deleted);

        var project = await service.GetByIdAsync(createdProject.Id);

        Assert.Null(project);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
    {
        var repository = new FakeProjectRepository();
        var service = new ProjectService(repository);

        var deleted = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(deleted);
    }
}
