# AGENTS.md

## Project Overview

ShNotes is a REST API for managing notes, built with ASP.NET Core.

The project follows a layered architecture with separation between domain logic, application use cases, infrastructure, caching, and presentation.

The main goal is to keep business logic independent from infrastructure and HTTP concerns.

---

## Tech Stack

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* MediatR
* AutoMapper
* Swagger / OpenAPI
* xUnit
* `IMemoryCache`

---

## Repository Structure

```text
src/
├── Core/
│   ├── ShNotes.Core/
│   └── ShNotes.UseCases/
│
├── Infra/
│   ├── ShNotes.Data/
│   └── ShNotes.Caching/
│
└── Presenters/
    └── ShNotes.WebApi/

tests/
└── ShNotes.Tests/
```

### `ShNotes.Core`

Contains domain logic.

This layer should not depend on:

* ASP.NET Core
* Entity Framework Core
* SQLite
* caching implementations
* Web API
* presentation-layer code

Domain entities, value objects, domain exceptions, and abstractions belong here.

### `ShNotes.UseCases`

Contains application logic.

Use cases are represented by commands/queries and their handlers.

MediatR is used to dispatch use cases.

Examples:

```text
Notes/
├── AddNote
├── GetNotes
├── GetNote
├── ChangeNoteName
├── ChangeNoteDescription
├── ChangeNoteStatus
└── RemoveNote

Users/
├── CreateUser
├── GetUser
└── SignIn
```

Do not put HTTP-specific logic into use cases.

### `ShNotes.Data`

Contains persistence infrastructure.

Responsibilities include:

* Entity Framework Core configuration
* DAO/database models
* repository implementations
* SQL scripts
* database access

Domain entities should not be coupled directly to EF Core models.

### `ShNotes.Caching`

Contains caching infrastructure.

The project currently uses in-memory caching.

Caching should remain separated from the core application logic.

Cache invalidation must happen when cached data becomes stale after mutations.

### `ShNotes.WebApi`

Contains the HTTP presentation layer.

Controllers should remain thin.

Controllers should:

1. receive HTTP requests;
2. validate/translate request data when necessary;
3. create the appropriate command/query;
4. send it through MediatR;
5. return the appropriate HTTP response.

Business logic should not be implemented inside controllers.

---

## Architecture Rules

### Dependency Direction

Dependencies should point inward.

```text
WebApi
  ↓
UseCases
  ↓
Core

Data ───────→ Core
Caching ────→ UseCases/Core
```

The domain must remain independent from infrastructure.

Do not introduce dependencies from `ShNotes.Core` to:

* EF Core
* ASP.NET Core
* MediatR
* SQLite
* caching implementations

unless there is a strong architectural reason.

---

## Domain Rules

Domain entities should protect their own invariants.

Prefer:

```csharp
public string Name { get; private set; }
```

over publicly mutable properties when external code should not modify the state directly.

Business rules belong inside the domain or appropriate use cases, not inside controllers or repositories.

Do not create an anemic domain model unnecessarily.

---

## Use Case Rules

Each meaningful application operation should have its own use case.

Prefer:

```text
ChangeNoteNameCommand
ChangeNoteNameCommandHandler
```

over putting unrelated operations into one large service.

Handlers should coordinate application logic rather than becoming god classes.

Do not access `DbContext` directly from Web API controllers.

Use abstractions provided by the application/domain layers.

---

## CQRS / MediatR

MediatR is used to separate request dispatching from request handling.

Queries should not modify application state.

Commands are responsible for state changes.

Examples:

```text
GetNotesQuery        → read
GetNoteQuery         → read

AddNoteCommand       → write
ChangeNoteNameCommand
ChangeNoteDescriptionCommand
ChangeNoteStatusCommand
RemoveNoteCommand
```

Do not introduce MediatR abstractions where they provide no architectural benefit.

---

## Data Access

Persistence models and domain models should remain separated.

Prefer:

```text
Domain Entity
     ↓
DAO
     ↓
EF Core
     ↓
Database
```

rather than exposing EF entities throughout the application.

Repository abstractions belong outside the infrastructure implementation.

Do not leak `DbContext`, EF-specific types, or SQLite-specific types into the domain layer.

---

## Mapping

AutoMapper is used for mapping between models.

Typical mappings include:

```text
Domain Entity ↔ DAO
Domain Entity → DTO
Request DTO → Command
```

Do not add unnecessary mapping layers when a simple explicit mapping is clearer.

---

## Caching

Caching is an optimization, not a source of truth.

The database remains the source of truth.

When a command changes data, all affected cached results must be considered for invalidation.

Be especially careful with filtered queries.

A mutation may invalidate multiple cached queries, for example:

```text
GetNotes()
GetNotes(filter = "abc")
GetNotes(filter = "test")
```

Do not introduce global/static mutable state merely to pass cache invalidation information between unrelated classes.

Prefer explicit dependencies and dedicated cache invalidation abstractions.

---

## Users and Authentication

The project contains user and credential domain models.

Credentials must never store plaintext passwords.

Password-related operations should use secure password hashing.

User-owned resources must be checked against the authenticated/requesting user where applicable.

Do not assume that receiving a note ID is sufficient authorization to modify it.

Authentication and authorization logic should remain separate from domain entities where possible.

---

## API

API endpoints should follow REST conventions where practical.

Use appropriate HTTP methods:

```text
GET     → read
POST    → create
PATCH   → partial update
DELETE  → delete
```

Do not expose internal domain or DAO models directly from controllers.

Use DTOs for API contracts.

API changes should be reflected in Swagger/OpenAPI where applicable.

---

## Error Handling

The application uses centralized error handling.

Do not add repetitive `try/catch` blocks to every controller.

Expected domain/application errors should be translated into appropriate HTTP responses by the centralized error-handling mechanism.

Do not silently swallow exceptions.

---

## Coding Style

Follow the existing code style.

Prefer:

* `sealed` classes when inheritance is not intended;
* explicit access modifiers;
* `private` setters;
* constructor injection;
* immutable/read-only state where appropriate;
* nullable reference types;
* asynchronous APIs for I/O;
* meaningful names;
* small focused classes.

Avoid:

* unnecessary abstractions;
* generic `Service` classes containing unrelated operations;
* god classes;
* static mutable state;
* service locator patterns;
* business logic inside controllers;
* leaking infrastructure concerns into Core.

Do not refactor unrelated code unless required for the requested change.

---

## Dependency Injection

Use ASP.NET Core dependency injection.

Prefer constructor injection.

Register dependencies in the appropriate layer's dependency injection configuration.

Do not resolve services manually with:

```csharp
IServiceProvider.GetService(...)
```

when normal dependency injection can be used.

Keep service lifetimes intentional.

Use `Scoped` for dependencies tied to a request/unit of work.

Use `Transient` for lightweight stateless handlers/services when appropriate.

Use `Singleton` only when the dependency is genuinely safe to share for the entire application lifetime.

---

## Testing

Tests are located under:

```text
tests/ShNotes.Tests/
```

When changing business logic, add or update tests where appropriate.

Prioritize testing:

* domain invariants;
* use cases;
* authorization rules;
* cache invalidation;
* important edge cases.

Do not write tests that merely duplicate implementation details.

---

## Database Changes

When changing persistence:

1. update the relevant DAO/model;
2. update EF configuration;
3. update SQL scripts if applicable;
4. update mappings;
5. update affected repositories/use cases;
6. update tests.

Do not modify only the database model while leaving mappings or SQL scripts inconsistent.

---

## Before Making Changes

Before implementing a change:

1. Inspect the existing implementation.
2. Identify the layer where the change belongs.
3. Reuse existing abstractions.
4. Check whether a similar use case already exists.
5. Avoid introducing a new abstraction if an existing one can be extended.
6. Check affected tests.
7. Keep the change focused.

Do not redesign the entire architecture for a small feature.

---

## Building and Testing

Restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run tests:

```bash
dotnet test
```

Run the API:

```bash
dotnet run --project src/Presenters/ShNotes.WebApi
```

After making code changes, prefer running at least:

```bash
dotnet build
dotnet test
```

---

## Git

Keep commits focused.

Do not modify unrelated files.

Do not rewrite git history unless explicitly requested.

Do not create commits automatically unless explicitly requested.

Before committing, inspect:

```bash
git status
git diff
```

Commit messages should describe the actual change.

Examples:

```text
Add user credentials
Add note cache invalidation
Implement note ownership
Add cached GetNotes handler
```

---

## Agent Behavior

When modifying the project:

* Follow the existing architecture before proposing a new one.
* Prefer the smallest correct change.
* Do not introduce unnecessary libraries.
* Do not replace existing patterns without a concrete reason.
* Do not silently change public API contracts.
* Do not silently change database behavior.
* Do not remove existing functionality to make implementation easier.
* Keep Core independent from infrastructure.
* Keep controllers thin.
* Keep use cases focused.
* Preserve existing naming conventions.
* Run build/tests after significant changes.

If an architectural trade-off is necessary, explain it briefly before making a large structural change.

The repository's existing code is the source of truth for conventions when this document does not explicitly define them.
