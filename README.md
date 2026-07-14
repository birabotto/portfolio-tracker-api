# Portfolio Tracker API

A professional REST API built with .NET, Entity Framework Core, PostgreSQL, Docker, and GitHub Actions.

This project was created as a portfolio project to demonstrate backend development skills, clean project organization, persistence with PostgreSQL, automated testing, Docker support, and CI/CD fundamentals.

## Features

- Create portfolio projects
- List all projects
- Get project by ID
- Update project information
- Delete projects
- PostgreSQL persistence with Entity Framework Core
- Repository pattern with interfaces
- Application service layer
- Unit tests with xUnit
- Dockerized API and database
- GitHub Actions CI pipeline
- Swagger/OpenAPI documentation
- Health check endpoint

## Tech Stack

- .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core
- PostgreSQL
- Docker
- Docker Compose
- xUnit
- GitHub Actions
- Swagger / OpenAPI

## Project Structure

```text
portfolio-tracker-api/
  src/
    PortfolioTracker.Api/
      Endpoints
      Program.cs

    PortfolioTracker.Application/
      DTOs
      Interfaces
      Services

    PortfolioTracker.Domain/
      Entities
      Enums

    PortfolioTracker.Infrastructure/
      Persistence
      Repositories

  tests/
    PortfolioTracker.Tests/

  .github/
    workflows/
      ci.yml

  Dockerfile
  docker-compose.yml
  README.md
```
