using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Application.DTOs;

public sealed record ProjectQueryParameters(
    string? Search,
    ProjectStatus? Status,
    int PageNumber = 1,
    int PageSize = 10);
