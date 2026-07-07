using PortfolioTracker.Domain.Enums;

namespace PortfolioTracker.Domain.Entities;

public sealed class Project
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string RepositoryUrl { get; private set; } = string.Empty;

    public string? DemoUrl { get; private set; }

    public string TechStack { get; private set; } = string.Empty;

    public ProjectStatus Status { get; private set; } = ProjectStatus.Planning;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; private set; }

    private Project()
    {
    }

    private Project(
        string name,
        string description,
        string repositoryUrl,
        string? demoUrl,
        string techStack)
    {
        Validate(name, description, repositoryUrl, demoUrl, techStack);

        Name = name.Trim();
        Description = description.Trim();
        RepositoryUrl = repositoryUrl.Trim();
        DemoUrl = string.IsNullOrWhiteSpace(demoUrl) ? null : demoUrl.Trim();
        TechStack = techStack.Trim();
        Status = ProjectStatus.Planning;
        CreatedAt = DateTime.UtcNow;
    }

    public static Project Create(
        string name,
        string description,
        string repositoryUrl,
        string? demoUrl,
        string techStack)
    {
        return new Project(name, description, repositoryUrl, demoUrl, techStack);
    }

    public void Update(
        string name,
        string description,
        string repositoryUrl,
        string? demoUrl,
        string techStack,
        ProjectStatus status)
    {
        Validate(name, description, repositoryUrl, demoUrl, techStack);

        Name = name.Trim();
        Description = description.Trim();
        RepositoryUrl = repositoryUrl.Trim();
        DemoUrl = string.IsNullOrWhiteSpace(demoUrl) ? null : demoUrl.Trim();
        TechStack = techStack.Trim();
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(
        string name,
        string description,
        string repositoryUrl,
        string? demoUrl,
        string techStack)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Project name is required.", nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentException("Project name must have at most 100 characters.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Project description is required.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(repositoryUrl))
        {
            throw new ArgumentException("Repository URL is required.", nameof(repositoryUrl));
        }

        if (!Uri.TryCreate(repositoryUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Repository URL must be a valid absolute URL.", nameof(repositoryUrl));
        }

        if (!string.IsNullOrWhiteSpace(demoUrl) &&
            !Uri.TryCreate(demoUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Demo URL must be a valid absolute URL.", nameof(demoUrl));
        }

        if (string.IsNullOrWhiteSpace(techStack))
        {
            throw new ArgumentException("Tech stack is required.", nameof(techStack));
        }
    }
}
