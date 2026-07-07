namespace PortfolioTracker.Application.DTOs;

public sealed record CreateProjectRequest(
    string Name,
    string Description,
    string RepositoryUrl,
    string? DemoUrl,
    string TechStack);
