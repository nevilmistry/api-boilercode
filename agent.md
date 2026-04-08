# Clean Architecture Structure

This repository now uses a clean-architecture application with four direct layers at the root.

## Layered Projects

- `Domain`
  - Enterprise entities only (`User`, `Role`, `BaseEntity`).
  - No dependencies on other layers.

- `Application`
  - Business logic and use-case services.
  - Service interfaces: `IUserService`, `IRoleService`.
  - Repository abstractions: `IUserRepository`, `IRoleRepository`.
  - Contracts/DTOs for request and response models.
  - Depends only on `Domain`.

- `Infrastructure`
  - EF Core data access implementation.
  - `AppDbContext`, repository implementations, migrations.
  - Implements application persistence abstractions.
  - Depends on `Application` and `Domain`.

- `Presentation`
  - ASP.NET Core Web API layer.
  - Controllers (`UsersController`, `RolesController`) and `Program.cs`.
  - Registers Application + Infrastructure via DI.
  - Depends on `Application` and `Infrastructure`.

## Request Flow

1. HTTP request enters `Presentation` controller.
2. Controller calls `Application` service interface.
3. Service applies business rules and uses repository abstractions.
4. `Infrastructure` repository accesses SQL Server via EF Core `AppDbContext`.
5. Response DTO is returned as JSON.

## Migrations

EF Core migrations for this architecture are in:

- `Infrastructure/Migrations`

## Solution

The solution file `genricRepository.slnx` points only to these four root layer projects.
