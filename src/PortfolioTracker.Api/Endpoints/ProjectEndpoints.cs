using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Api.Endpoints;

public static class ProjectEndpoints
{
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/projects")
            .WithTags("Projects");

        group.MapGet("/", async (
            string? search,
            ProjectStatus? status,
            int? pageNumber,
            int? pageSize,
            IProjectService projectService,
            CancellationToken cancellationToken) =>
        {
            var queryParameters = new ProjectQueryParameters(
                search,
                status,
                pageNumber ?? 1,
                pageSize ?? 10);

            var projects = await projectService.GetPagedAsync(queryParameters, cancellationToken);

            return Results.Ok(projects);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IProjectService projectService,
            CancellationToken cancellationToken) =>
        {
            var project = await projectService.GetByIdAsync(id, cancellationToken);

            return project is null
                ? Results.NotFound()
                : Results.Ok(project);
        });

        group.MapPost("/", async (
            CreateProjectRequest request,
            IProjectService projectService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var project = await projectService.CreateAsync(request, cancellationToken);

                return Results.Created($"/api/projects/{project.Id}", project);
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
            catch (InvalidOperationException exception)
            {
                return Results.Conflict(new { message = exception.Message });
            }
        });

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateProjectRequest request,
            IProjectService projectService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var project = await projectService.UpdateAsync(id, request, cancellationToken);

                return project is null
                    ? Results.NotFound()
                    : Results.Ok(project);
            }
            catch (ArgumentException exception)
            {
                return Results.BadRequest(new { message = exception.Message });
            }
        });

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IProjectService projectService,
            CancellationToken cancellationToken) =>
        {
            var deleted = await projectService.DeleteAsync(id, cancellationToken);

            return deleted
                ? Results.NoContent()
                : Results.NotFound();
        });

        return app;
    }
}
