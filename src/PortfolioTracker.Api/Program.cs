using PortfolioTracker.Api.Endpoints;
using PortfolioTracker.Application;
using PortfolioTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    service = "PortfolioTracker.Api",
    timestamp = DateTime.UtcNow
}))
.WithTags("Health");

app.MapProjectEndpoints();

app.Run();

public partial class Program
{
}
