using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Application.DTOs;

public sealed record UpdateProjectRequest(
    string Name,
    string Description,
    string RepositoryUrl,
    string? DemoUrl,
    string TechStack,
    ProjectStatus Status);
