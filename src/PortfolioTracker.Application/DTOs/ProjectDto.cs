using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Application.DTOs;

public sealed record ProjectDto(
    Guid Id,
    string Name,
    string Description,
    string RepositoryUrl,
    string? DemoUrl,
    string TechStack,
    ProjectStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
