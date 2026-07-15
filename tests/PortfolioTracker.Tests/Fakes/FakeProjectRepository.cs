using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;

namespace PortfolioTracker.Tests.Fakes;

internal sealed class FakeProjectRepository : IProjectRepository
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    private readonly List<Project> _projects = [];

    public Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Project>>(_projects);
    }

    public Task<PagedResult<Project>> GetPagedAsync(
        ProjectQueryParameters queryParameters,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = queryParameters.PageNumber < 1
            ? DefaultPageNumber
            : queryParameters.PageNumber;

        var pageSize = queryParameters.PageSize switch
        {
            < 1 => DefaultPageSize,
            > MaxPageSize => MaxPageSize,
            _ => queryParameters.PageSize
        };

        var query = _projects.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            var search = queryParameters.Search.Trim();

            query = query.Where(project =>
                project.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                project.Description.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                project.TechStack.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        if (queryParameters.Status is not null)
        {
            query = query.Where(project => project.Status == queryParameters.Status.Value);
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(project => project.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        var result = new PagedResult<Project>(
            items,
            pageNumber,
            pageSize,
            totalCount,
            totalPages);

        return Task.FromResult(result);
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
