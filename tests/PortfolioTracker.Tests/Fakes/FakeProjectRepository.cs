using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Tests.Fakes;

internal sealed class FakeProjectRepository : IProjectRepository
{
    private readonly List<Project> _projects = [];

    public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Project>>(_projects);
    }

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var project = _projects.FirstOrDefault(project => project.Id == id);

        return Task.FromResult(project);
    }

    public Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        _projects.Add(project);

        return Task.FromResult(project);
    }

    public Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
    {
        _projects.Remove(project);

        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var exists = _projects.Any(project =>
            string.Equals(project.Name, name, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(exists);
    }
}
