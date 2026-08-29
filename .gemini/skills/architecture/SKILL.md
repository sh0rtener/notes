---

name: architecture
description:
Expertise in the ShNotes architecture. Use when implementing features,
refactoring layers, introducing abstractions, changing dependencies,
modifying use cases, repositories, caching, domain models, or infrastructure.
-----------------------------------------------------------------------------

# ShNotes Architecture Skill

You are working on the ShNotes backend.

Your primary responsibility is to preserve the existing layered architecture and prevent infrastructure concerns from leaking into the domain.

## Rules

### Layer responsibilities

* `ShNotes.Core` — domain model and domain abstractions.
* `ShNotes.UseCases` — application/use-case logic.
* `ShNotes.Data` — persistence and EF Core.
* `ShNotes.Caching` — caching infrastructure.
* `ShNotes.WebApi` — HTTP presentation.

Respect dependency direction.

```text
WebApi
  ↓
UseCases
  ↓
Core

Data ─────→ Core
Caching ──→ UseCases/Core
```

Never introduce a dependency from Core to WebApi, EF Core, SQLite, or other infrastructure.

## Before changing architecture

Always inspect the existing implementation first.

Prefer extending an existing abstraction over introducing another abstraction with the same responsibility.

Do not introduce:

* unnecessary generic repositories;
* service locator;
* static mutable state;
* god services;
* business logic in controllers;
* infrastructure types in Core.

## Use cases

Each application operation should have a focused command/query and handler.

Use MediatR consistently with the existing project.

Queries must not modify state.

Commands perform mutations.

## Domain

Keep domain invariants inside domain entities/value objects where appropriate.

Prefer encapsulated state:

```csharp
public string Name { get; private set; }
```

Do not expose mutable domain state without a reason.

## Persistence

Keep DAO/database models separate from domain entities.

Do not expose EF Core models through the API.

Do not make domain entities depend on `DbContext`, `DbSet`, EF attributes, or SQLite-specific types.

## Caching

Caching is an optimization.

The database remains the source of truth.

When modifying data, identify every affected cached query and invalidate it.

Pay special attention to filtered `GetNotes` queries.

Do not solve cache invalidation with global/static mutable state unless explicitly required.

## Controllers

Controllers must remain thin.

They should translate HTTP requests into commands/queries and dispatch them through MediatR.

Do not implement business logic inside controllers.

## Refactoring

When refactoring:

1. Preserve current behavior.
2. Make the smallest architectural change necessary.
3. Avoid unrelated cleanup.
4. Keep public API contracts unchanged unless requested.
5. Run the build and relevant tests afterward.

When there are multiple valid architectural approaches, prefer the one that matches patterns already present in ShNotes.
