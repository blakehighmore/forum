# Forum API

A backend REST API for a discussion forum, built as a deep-dive learning project in **ASP.NET Core**.
It implements a full domain (categories → topics → posts), production-style authentication and
authorization, raw SQL access with Dapper, typo-tolerant full-text search, and integration tests
against a real database.

## Tech stack

- **.NET 9** / ASP.NET Core Web API
- **PostgreSQL** (Npgsql)
- **EF Core** — migrations, LINQ, Fluent API + **Dapper** — raw SQL for selected read paths
- **FluentValidation**
- **JWT** authentication — access + refresh tokens
- **pg_trgm** — trigram fuzzy search with a GIN index
- **xUnit + Testcontainers** — integration tests against a real, throwaway Postgres

## Features

- **Domain CRUD:** Categories, Topics, Tags, Posts, Profiles, Reactions
- **Many-to-many** Topics ↔ Tags
- **Value object** `Color` (hex), persisted via an EF Core value converter
- **Authentication & authorization:**
  - Register / login, passwords hashed with **BCrypt**
  - **JWT access tokens** + **refresh tokens** — rotation on refresh, revoke on logout, stored in the DB
  - **Role-based** authorization (User / Moderator / Admin)
  - **Resource-based** ownership — "edit only your own", with a moderator/admin override
- **Search:** fuzzy full-text post search via PostgreSQL **`pg_trgm`** (typo-tolerant, GIN-indexed)
- **Dapper** for selected read queries (raw, parameterized SQL) alongside EF Core
- Global exception handling → **ProblemDetails** (consistent 400 / 401 / 403 / 404 / 409)
- Pagination, filtering and sorting
- **Integration tests** with Testcontainers — a fresh Postgres container per test

## Domain model

- `User` 1—1 `Profile` (optional)
- `Category` 1—∞ `Topic` 1—∞ `Post`
- `Topic` ∞—∞ `Tag`
- `Post` 1—∞ `PostReaction` (Like / Dislike, unique per user + post)
- `User` 1—∞ `RefreshToken`

## API overview

| Area | Endpoints |
|---|---|
| Auth | `POST /api/auth/register`, `/login`, `/refresh`, `/logout` |
| Categories | `GET / POST / PUT / DELETE /api/categories` — writes require **Admin** |
| Topics | `GET / POST / PUT / DELETE /api/topics` — edit/delete: author or **Moderator/Admin** |
| Tags | `GET / POST / PUT / DELETE /api/tags` — writes require **Admin** |
| Posts | `GET / POST / PUT / DELETE /api/posts`, `GET /api/posts/by-topic/{id}` (Dapper), `/search-raw` (Dapper), `/search-fuzzy` (pg_trgm) |
| Reactions | `POST /api/reactions` — toggle Like/Dislike |
| Profiles | `GET / PUT /api/profiles` — your own profile |

## Getting started

### Prerequisites

- .NET 9 SDK
- PostgreSQL running locally, with a database named `forum_db`
- Docker (only required to run the integration tests)

### Configuration

The connection string lives in `appsettings.json` (`ConnectionStrings:DefaultConnection`).
The JWT signing key is kept out of source control — provide it via user-secrets:

```bash
dotnet user-secrets set "Jwt:Key" "<your-32+-character-secret>"
```

### Run

```bash
dotnet ef database update     # apply migrations (creates schema + pg_trgm extension)
dotnet run                    # API on http://localhost:5049
```

### Tests

```bash
dotnet test                   # spins up a throwaway Postgres via Testcontainers (Docker must be running)
```

## Project structure

```
backend/             # API
  Controllers/       # endpoints
  Services/          # business logic (EF Core + Dapper)
  Models/            # domain entities + value objects
  DTOs/  Validators/ # contracts + FluentValidation
  Data/              # AppDbContext, IDbConnectionFactory (Dapper)
  Migrations/
backend.Tests/       # integration tests (xUnit + Testcontainers)
```

---

Built to learn ASP.NET Core backend development end to end: EF Core & raw SQL, JWT auth with roles and
refresh tokens, PostgreSQL-specific features, and real-database integration testing.
