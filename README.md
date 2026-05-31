# Dogshouseservice

Dogshouseservice is a .NET 8 ASP.NET Core Web API for working with dog records. The solution follows a layered architecture with separate projects for the API, application logic, domain model, and infrastructure/data access.

## Solution structure

- `Dogshouseservice.Api` - ASP.NET Core Web API, controllers, Swagger, rate limiting, and dependency injection.
- `Dogshouseservice.Application` - DTOs, service abstractions, and business logic.
- `Dogshouseservice.Domain` - Core domain entities.
- `Dogshouseservice.Infrastructure` - Entity Framework Core persistence, repositories, and migrations.
- `Dogshouseservice.Tests` - Automated tests for the API, services, and repository layer.

## Prerequisites

- .NET SDK 8.0 (`global.json` pins the solution to `8.0.0`)
- SQL Server LocalDB or another SQL Server instance
- Optional: `dotnet-ef` for applying EF Core migrations

## Configuration

The API uses the connection string defined in `Dogshouseservice.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\local;Database=Dogs;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

If you want to use a different database, update `DefaultConnection` in `Dogshouseservice.Api/appsettings.json` or override it with environment-specific configuration.

## Database setup

The database model is managed by Entity Framework Core migrations in `Dogshouseservice.Infrastructure/Migrations`.

If you need to apply migrations manually, run:

```powershell
dotnet ef database update --project .\Dogshouseservice.Infrastructure\Dogshouseservice.Infrastructure.csproj --startup-project .\Dogshouseservice.Api\Dogshouseservice.Api.csproj
```

If `dotnet ef` is not installed yet, install it first:

```powershell
dotnet tool install --global dotnet-ef
```

## Run the API

From the repository root:

```powershell
dotnet run --project .\Dogshouseservice.Api\Dogshouseservice.Api.csproj
```

When running locally with the default launch profile, Swagger is available at:

- `http://localhost:5139/swagger`
- `https://localhost:7183/swagger`

## Run tests

```powershell
dotnet test
```

## API endpoints

The controller exposes routes directly at the root path, so there is no `/api` prefix.

### `GET /ping`

Simple health check endpoint.

**Response**

```text
Dogshouseservice.Version1.0.0
```

### `GET /dogs`

Returns a paginated, sortable list of dogs.

**Query parameters**

- `attribute` - optional sort field: `name`, `color`, `tailLength`, or `weight`
- `order` - optional sort direction: `asc` or `desc`
- `pageNumber` - page number, default `1`
- `pageSize` - page size, default `10`, max `100`

**Example**

`/dogs?attribute=name&order=asc&pageNumber=1&pageSize=10`

### `POST /dog`

Creates a new dog.

**Request body**

```json
{
  "name": "Rex",
  "color": "Brown",
  "tailLength": 12,
  "weight": 20
}
```

**Validation rules**

- `name` is required
- `color` is required
- `tailLength` must be at least `1`
- `weight` must be at least `1`

## Rate limiting

The API uses a fixed-window rate limiter configured in `Program.cs`:

- 10 requests per second per window
- Rejected requests return HTTP `429 Too Many Requests`

## Development notes

- The API project is the startup project and wires up `AppDbContext`, `DogRepository`, and `DogService` through dependency injection.
- Swagger is enabled in the Development environment.
- Tests use the `Dogshouseservice.Tests` project and reference the API, Application, Domain, and Infrastructure layers.


