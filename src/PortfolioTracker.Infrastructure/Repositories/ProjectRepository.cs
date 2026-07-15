using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Application.Interfaces;
using PortfolioTracker.Domain.Entities;
using PortfolioTracker.Infrastructure.Persistence;

namespace PortfolioTracker.Infrastructure.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private const int DefaultPageNumber = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;

    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Project>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderByDescending(project => project.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Project>> GetPagedAsync(
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

        var query = _context.Projects
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            var searchTerm = $"%{queryParameters.Search.Trim()}%";

            query = query.Where(project =>
                EF.Functions.ILike(project.Name, searchTerm) ||
                EF.Functions.ILike(project.Description, searchTerm) ||
                EF.Functions.ILike(project.TechStack, searchTerm));
        }

        if (queryParameters.Status is not null)
        {
            query = query.Where(project => project.Status == queryParameters.Status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(project => project.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Project>(
            items,
            pageNumber,
            pageSize,
            totalCount,
            totalPages);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
    }

    public async Task<Project> AddAsync(Project project, CancellationToken cancellationToken = default)
    {
        await _context.Projects.AddAsync(project, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.Projects
            .AsNoTracking()
            .AnyAsync(project => project.Name.ToLower() == normalizedName, cancellationToken);
    }
}
