using FluentValidation;
using PortfolioTracker.Application.DTOs;
using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Application.Validators;

public sealed class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .MaximumLength(100)
            .WithMessage("Project name must have at most 100 characters.");

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage("Project description is required.")
            .MaximumLength(1000)
            .WithMessage("Project description must have at most 1000 characters.");

        RuleFor(request => request.RepositoryUrl)
            .NotEmpty()
            .WithMessage("Repository URL is required.")
            .Must(BeAValidAbsoluteUrl)
            .WithMessage("Repository URL must be a valid absolute URL.")
            .MaximumLength(500)
            .WithMessage("Repository URL must have at most 500 characters.");

        RuleFor(request => request.DemoUrl)
            .Must(BeNullOrAValidAbsoluteUrl)
            .WithMessage("Demo URL must be a valid absolute URL.")
            .MaximumLength(500)
            .WithMessage("Demo URL must have at most 500 characters.");

        RuleFor(request => request.TechStack)
            .NotEmpty()
            .WithMessage("Tech stack is required.")
            .MaximumLength(300)
            .WithMessage("Tech stack must have at most 300 characters.");

        RuleFor(request => request.Status)
            .IsInEnum()
            .WithMessage($"Status must be one of: {string.Join(", ", Enum.GetNames<ProjectStatus>())}.");
    }

    private static bool BeAValidAbsoluteUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private static bool BeNullOrAValidAbsoluteUrl(string? url)
    {
        return string.IsNullOrWhiteSpace(url) ||
               Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
