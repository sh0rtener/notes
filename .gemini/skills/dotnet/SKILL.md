---

name: dotnet
description:
Expertise in C# and ASP.NET Core development for ShNotes. Use when writing,
modifying, debugging, or reviewing C# code, dependency injection, MediatR,
EF Core, ASP.NET Core controllers, DTOs, mappings, or application services.
---------------------------------------------------------------------------

# ShNotes .NET Skill

Write C# code that follows the existing ShNotes conventions.

## Target

The project targets:

* .NET 8
* ASP.NET Core
* nullable reference types

Do not introduce APIs requiring a newer framework unless explicitly requested.

## C# style

Prefer:

* `sealed` classes when inheritance is not intended;
* explicit access modifiers;
* constructor injection;
* `private` setters;
* nullable reference types;
* async I/O;
* focused classes;
* meaningful names.

Avoid:

* unnecessary abstractions;
* static mutable state;
* service locator;
* large service classes;
* unnecessary inheritance;
* duplicated logic.

## Dependency Injection

Use constructor injection.

Do not resolve dependencies manually through `IServiceProvider` unless there is a specific architectural reason.

Choose service lifetimes deliberately.

Prefer `Scoped` for request/unit-of-work scoped dependencies.

Use `Transient` for stateless lightweight components where appropriate.

Use `Singleton` only for genuinely application-wide thread-safe state.

## MediatR

Follow the existing command/query pattern.

Example:

```csharp
public sealed record GetNoteQuery(int Id) : IRequest<NoteDto>;
```

Handlers should remain focused.

Do not put HTTP concerns inside handlers.

## EF Core

Keep EF Core inside `ShNotes.Data`.

Do not return EF entities directly from API endpoints.

Use the existing DAO/repository abstractions.

When changing database-related code, inspect:

* DAO;
* EF configuration;
* mappings;
* repository;
* SQL scripts;
* affected use cases;
* tests.

## AutoMapper

Reuse existing mappings.

Do not create redundant mapping profiles.

If explicit mapping is substantially clearer for a small transformation, prefer clarity over unnecessary abstraction.

## ASP.NET Core

Controllers should be thin.

Use proper HTTP methods and status codes.

Do not duplicate global exception handling inside controllers.

Do not expose internal domain/DAO types as API contracts.

## Debugging

When fixing a bug:

1. Read the complete relevant code path.
2. Identify the actual root cause.
3. Prefer fixing the cause over masking the exception.
4. Avoid unrelated refactoring.
5. Build the project.
6. Run affected tests.

Do not blindly change DI lifetimes or add registrations just to make an exception disappear.
