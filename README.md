# NZWalks API

A REST API for browsing and managing New Zealand walking tracks, built with ASP.NET Core 8 and Entity Framework Core. Includes a small MVC client app that consumes the API.

Built while working through an ASP.NET Core Web API course, then extended with Azure deployment using passwordless authentication.

## Features

- **REST API** with full CRUD for Regions and Walks
- **Filtering, sorting and pagination** on the Walks endpoint
- **JWT authentication** backed by ASP.NET Core Identity, with `Reader` and `Writer` roles seeded via migration
- **Repository pattern** separating controllers from data access
- **AutoMapper** for domain-to-DTO mapping
- **Serilog** logging to console and a daily rolling file
- **Global exception handling** via custom middleware, returning a correlation ID
- **Model validation** through a reusable action filter
- **Image upload** with local file storage, served over static files
- **Swagger / OpenAPI** with bearer token support
- **Code-first migrations** across two separate DbContexts

## Tech stack

| Area | Technology |
|---|---|
| Framework | ASP.NET Core 8 (`net8.0`) |
| Data access | Entity Framework Core 8, SQL Server |
| Auth | ASP.NET Core Identity, JWT bearer tokens |
| Mapping | AutoMapper |
| Logging | Serilog (console + file sinks) |
| API docs | Swashbuckle / Swagger |
| Client | ASP.NET Core MVC, Bootstrap |
| Hosting | Azure App Service, Azure SQL Database |

## Projects

```
NZWalks.API/   ASP.NET Core Web API — controllers, repositories, EF Core contexts, migrations
NZWalks.UI/    ASP.NET Core MVC client that calls the API over HTTP
```

## Endpoints

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/Auth/Register` | Register a user and assign roles |
| `POST` | `/api/Auth/Login` | Exchange credentials for a JWT |
| `GET` | `/api/Regions` | List all regions |
| `GET` | `/api/Regions/{id}` | Get a region by id |
| `POST` | `/api/Regions` | Create a region |
| `PUT` | `/api/Regions/{id}` | Update a region |
| `DELETE` | `/api/Regions/{id}` | Delete a region |
| `GET` | `/api/Walks` | List walks — supports `filterOn`, `filterQuery`, `sortBy`, `isAscending`, `pageNumber`, `pageSize` |
| `GET` | `/api/Walks/{id}` | Get a walk by id |
| `POST` | `/api/Walks` | Create a walk |
| `PUT` | `/api/Walks/{id}` | Update a walk |
| `DELETE` | `/api/Walks/{id}` | Delete a walk |
| `POST` | `/api/Images/Upload` | Upload an image (multipart form data) |

Example filtered request:

```
GET /api/Walks?filterOn=Name&filterQuery=Park&sortBy=LengthInKm&isAscending=true&pageNumber=1&pageSize=10
```

## Getting started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express or full) — or an Azure SQL Database

### 1. Clone and restore

```bash
git clone https://github.com/alvee-unb/NZWalks.git
cd NZWalks
dotnet restore
dotnet tool restore
```

### 2. Configure secrets

Configuration values are not committed. Set them with [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
cd NZWalks.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:NZWalksConnectionString" "Server=localhost;Database=NZWalksDb;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "ConnectionStrings:NZWalksAuthConnectionString" "Server=localhost;Database=NZWalksAuthDb;Trusted_Connection=True;TrustServerCertificate=True;"
dotnet user-secrets set "Jwt:Key" "a-development-key-of-at-least-32-bytes"
dotnet user-secrets set "Jwt:Issuer" "https://localhost:7081"
dotnet user-secrets set "Jwt:Audience" "https://localhost:7081"
```

The `Jwt:Key` must be at least 32 bytes for HMAC-SHA256 signing.

### 3. Apply migrations

Two DbContexts, so two commands:

```bash
dotnet ef database update --context NZWalksDbContext
dotnet ef database update --context NZWalksAuthDbContext
```

The auth migration seeds the `Reader` and `Writer` roles.

### 4. Run

```bash
dotnet run --project NZWalks.API
```

Swagger UI is available at `https://localhost:7081/swagger` in the Development environment.

To run the MVC client alongside it, start both projects from the solution.

### 5. Authenticate

1. `POST /api/Auth/Register` with a username, password and roles (`["Reader"]` or `["Reader","Writer"]`)
2. `POST /api/Auth/Login` to receive a JWT
3. Click **Authorize** in Swagger and paste `Bearer <token>`

## Deploying to Azure

The API runs on Azure App Service with Azure SQL Database, connected via **managed identity** rather than a stored password.

1. Enable a system-assigned identity on the App Service
2. Set yourself as the SQL server's Microsoft Entra admin
3. Create a contained database user for the App Service:

```sql
   CREATE USER [your-app-service-name] FROM EXTERNAL PROVIDER;
   ALTER ROLE db_datareader ADD MEMBER [your-app-service-name];
   ALTER ROLE db_datawriter ADD MEMBER [your-app-service-name];
```

4. Add connection strings in App Service configuration, type `SQLAzure`, using the key names `NZWalksConnectionString` and `NZWalksAuthConnectionString`:

```
   Server=tcp:<server>.database.windows.net;Database=<database>;Authentication=Active Directory Default;
```

5. Add `Jwt:Key`, `Jwt:Issuer` and `Jwt:Audience` as app settings

Migrations are applied separately rather than during publish, because Web Deploy's connection-string parser does not understand the `Authentication=Active Directory Default` keyword:

```bash
dotnet ef database update --context NZWalksDbContext --connection "<azure-connection-string>"
dotnet ef database update --context NZWalksAuthDbContext --connection "<azure-connection-string>"
```

## Project structure

```
NZWalks.API/
├── Controllers/          API endpoints
├── Data/                 EF Core DbContexts
├── Models/
│   ├── Domain/           Entity models
│   └── DTO/              Request and response contracts
├── Repositories/         Data access abstractions and implementations
├── Mappings/             AutoMapper profiles
├── Middlewares/          Global exception handler
├── CustomActionFilter/   Model validation filter
└── Migrations/           EF Core migrations (auth migrations under NZWalksAuthDb/)
```
