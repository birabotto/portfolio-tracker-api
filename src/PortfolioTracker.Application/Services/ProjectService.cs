using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Application.Services;

public sealed class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IReadOnlyList<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _projectRepository.GetAllAsync(cancellationToken);

        return projects
            .Select(MapToDto)
            .ToList();
    }

    public async Task<ProjectDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);

        return project is null ? null : MapToDto(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var exists = await _projectRepository.ExistsByNameAsync(request.Name, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("A project with this name already exists.");
        }

        var project = Project.Create(
            request.Name,
            request.Description,
            request.RepositoryUrl,
            request.DemoUrl,
            request.TechStack);

        var createdProject = await _projectRepository.AddAsync(project, cancellationToken);

        return MapToDto(createdProject);
    }

    public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);

        if (project is null)
        {
            return null;
        }

        project.Update(
            request.Name,
            request.Description,
            request.RepositoryUrl,
            request.DemoUrl,
            request.TechStack,
            request.Status);

        await _projectRepository.UpdateAsync(project, cancellationToken);

        return MapToDto(project);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _projectRepository.GetByIdAsync(id, cancellationToken);

        if (project is null)
        {
            return false;
        }

        await _projectRepository.DeleteAsync(project, cancellationToken);

        return true;
    }

    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto(
            project.Id,
            project.Name,
            project.Description,
            project.RepositoryUrl,
            project.DemoUrl,
            project.TechStack,
            project.Status,
            project.CreatedAt,
            project.UpdatedAt);
    }
}
