---

name: testing
description:
Expertise in testing ShNotes. Use when adding tests, fixing failing tests,
analyzing test coverage, testing use cases, domain logic, caching,
authorization, repositories, or API behavior.
---------------------------------------------

# ShNotes Testing Skill

Tests should verify behavior rather than implementation details.

## Test priority

Prioritize:

1. Domain invariants
2. Use cases
3. Authorization/ownership
4. Cache invalidation
5. Repository behavior
6. API behavior

## Unit tests

Prefer unit tests for:

* domain entities;
* value objects;
* commands/queries;
* handlers;
* business rules.

Mock only external dependencies when appropriate.

Do not mock every class simply because it is technically possible.

## Integration tests

Use integration tests when correctness depends on:

* EF Core;
* database behavior;
* repository implementation;
* HTTP pipeline;
* middleware;
* serialization.

Do not replace integration tests with large amounts of mocks when the actual integration is what needs verification.

## Cache tests

Whenever cache invalidation is changed, test both:

```text
read → cache miss → store
read → cache hit
mutation → invalidate
read → cache miss → fresh value
```

For filtered queries, test multiple different filters.

## Authorization tests

For user-owned notes verify:

```text
owner → allowed
different user → denied
missing user → denied
missing note → not found
```

Do not test only the happy path.

## Regression tests

When fixing a bug:

1. Reproduce the bug.
2. Add a regression test.
3. Fix the implementation.
4. Verify the test fails before the fix and passes afterward when practical.

## Test quality

Avoid tests that depend on:

* private implementation details;
* exact internal method calls;
* unnecessary invocation counts.

Prefer assertions about observable behavior.

## Running tests

Use:

```bash
dotnet test
```

After significant changes also run:

```bash
dotnet build
dotnet test
```
