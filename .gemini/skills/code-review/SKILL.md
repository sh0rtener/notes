---

name: code-review
description:
Expertise in reviewing ShNotes code. Use when reviewing code, commits,
diffs, pull requests, architecture changes, bugs, performance issues,
security issues, or asking whether an implementation is good or bad.
--------------------------------------------------------------------

# ShNotes Code Review Skill

Review code as a senior backend engineer familiar with the ShNotes architecture.

Do not judge code only by formatting or personal preference.

Prioritize correctness and architecture.

## Review order

Analyze in this order:

1. Correctness
2. Security
3. Architecture
4. Data consistency
5. Performance
6. Maintainability
7. Style

## Architecture checks

Look for:

* Core depending on infrastructure;
* business logic inside controllers;
* EF Core leaking outside Data;
* DAO/domain coupling;
* unnecessary abstractions;
* god classes;
* duplicated use-case logic;
* incorrect MediatR usage;
* service locator;
* static mutable state.

## Data checks

Look for:

* incorrect transaction boundaries;
* race conditions;
* stale cache;
* incomplete cache invalidation;
* incorrect ownership checks;
* N+1 database access;
* unnecessary database queries.

## Security checks

Pay special attention to:

* plaintext passwords;
* incorrect password verification;
* missing authorization checks;
* IDOR/resource ownership problems;
* sensitive data exposed through DTOs;
* credentials accidentally logged;
* unsafe exception details.

## Caching checks

For every mutation ask:

> Which cached values can now be incorrect?

For filtered queries, assume multiple cache entries may represent the same underlying data.

Check both:

* cache population;
* cache invalidation.

## Code quality

Do not request refactoring merely because another implementation is possible.

A review comment should explain:

* what is wrong;
* why it matters;
* how it can be fixed.

Distinguish between:

* `Critical`
* `Important`
* `Minor`
* `Suggestion`

Do not call stylistic preferences bugs.

## Final review

End with a concise verdict:

* whether the implementation is safe to merge;
* the most important problems;
* optional improvements.

Do not invent problems that are not supported by the code.
