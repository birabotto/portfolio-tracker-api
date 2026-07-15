using System.Net;
using System.Net.Http.Json;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Tests.Integration;

public sealed class ProjectEndpointsTests : IClassFixture<PortfolioTrackerApiFactory>
{
    private readonly HttpClient _client;

    public ProjectEndpointsTests(PortfolioTrackerApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnCreated_WhenRequestIsValid()
    {
        var request = new CreateProjectRequest(
            $"Portfolio Tracker API {Guid.NewGuid()}",
            "A professional .NET API for tracking developer portfolio projects.",
            "https://github.com/example/portfolio-tracker-api",
            null,
            ".NET, EF Core, PostgreSQL, Docker, GitHub Actions");

        var response = await _client.PostAsJsonAsync("/api/projects", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var project = await response.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(project);
        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal(request.Name, project.Name);
        Assert.Equal(ProjectStatus.Planning, project.Status);
    }

    [Fact]
    public async Task GetProjects_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/projects");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var projects = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();

        Assert.NotNull(projects);
    }

    [Fact]
    public async Task GetProjectById_ShouldReturnOk_WhenProjectExists()
    {
        var createRequest = new CreateProjectRequest(
            $"Integration Test Project {Guid.NewGuid()}",
            "Project created during integration testing.",
            "https://github.com/example/integration-test-project",
            null,
            ".NET, EF Core, PostgreSQL, Testcontainers");

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(createdProject);

        var getResponse = await _client.GetAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var project = await getResponse.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(project);
        Assert.Equal(createdProject.Id, project.Id);
    }

    [Fact]
    public async Task UpdateProject_ShouldReturnOk_WhenProjectExists()
    {
        var createRequest = new CreateProjectRequest(
            $"Project To Update {Guid.NewGuid()}",
            "Project before update.",
            "https://github.com/example/project-to-update",
            null,
            ".NET");

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);

        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(createdProject);

        var updateRequest = new UpdateProjectRequest(
            "Updated Integration Project",
            "Project updated during integration testing.",
            "https://github.com/example/updated-integration-project",
            "https://example.com",
            ".NET, EF Core, PostgreSQL",
            ProjectStatus.Completed);

        var updateResponse = await _client.PutAsJsonAsync($"/api/projects/{createdProject.Id}", updateRequest);

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedProject = await updateResponse.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(updatedProject);
        Assert.Equal(updateRequest.Name, updatedProject.Name);
        Assert.Equal(ProjectStatus.Completed, updatedProject.Status);
        Assert.NotNull(updatedProject.UpdatedAt);
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnNoContent_WhenProjectExists()
    {
        var createRequest = new CreateProjectRequest(
            $"Project To Delete {Guid.NewGuid()}",
            "Project created to be deleted.",
            "https://github.com/example/project-to-delete",
            null,
            ".NET, PostgreSQL");

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);

        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectDto>();

        Assert.NotNull(createdProject);

        var deleteResponse = await _client.DeleteAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/projects/{createdProject.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnBadRequest_WhenRepositoryUrlIsInvalid()
    {
        var request = new CreateProjectRequest(
            $"Invalid Project {Guid.NewGuid()}",
            "Project with invalid repository URL.",
            "invalid-url",
            null,
            ".NET");

        var response = await _client.PostAsJsonAsync("/api/projects", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
