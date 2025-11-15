
# GitHub Copilot & C# Coding Guidelines

This document provides rules and best practices for using GitHub Copilot in the UnambitiousFx repository, with a focus on C# code quality, maintainability, and complexity management.

## Copilot Usage
- Use Copilot for boilerplate, repetitive code, and documentation comments.
- Always review Copilot suggestions for correctness, security, and project fit.
- Validate generated code with unit tests and benchmarks.
- Refactor Copilot code to match repository style and architecture.

## C# Best Practices

### Code Complexity
- **Cyclomatic complexity:** Keep methods ≤ 10 for business logic, ≤ 15 for core internals. Split into smaller methods if exceeded.
- **Cognitive complexity:** Target ≤ 15. Refactor nested conditionals into guard clauses or use polymorphism.
- **Method length:** Prefer < 50 lines; orchestrators < 80 lines with clear regions.
- **Parameter count:** Prefer ≤ 5. Use parameter objects/records for larger sets.
- **Class focus:** Single responsibility. Split concerns into multiple files/classes.
- **Partial classes:** Group related members, avoid hidden cross-file coupling.

### File Organization
- Split large files into smaller, focused files by responsibility.
- Use namespaces and folders to organize code by domain, feature, or layer.
- Avoid putting unrelated types in the same file.

### Design Patterns
- Use design patterns (e.g., Strategy, Factory, Mediator, CQRS) when they improve clarity, testability, or extensibility.
- Prefer composition over inheritance. Use sealed classes unless inheritance is required.
- For Result flows, use Match/Map/Tap combinators over deeply nested if/else.

### General C# Style
- Prefer pure functions and minimize side effects.
- Use async-friendly patterns; avoid sync-over-async.
- Use records for immutable data and requests.
- Use ValueTask for hot paths when appropriate.
- Use explicit error handling; reserve exceptions for truly exceptional cases.
- Use PascalCase for public types/members, camelCase for locals/parameters, _prefix for private fields.
- Rely on ImplicitUsings; add explicit usings only when necessary.
- Avoid unguarded null dereference; document use of null-forgiving (!).

### Performance & Safety
- Use ReadOnly collections for exposures; avoid unnecessary allocations.
- Avoid defensive copies of large collections; expose IReadOnly interfaces.
- Avoid reflection on open generics; annotate with [DynamicallyAccessedMembers] if needed.
- Keep expression trees out of runtime paths unless compiled at build-time.

### Testing & Documentation
- Keep tests and docs in sync with code changes.
- Use xUnit and custom assertions/helpers from Core.XUnit.
- Document public APIs and complex logic with XML comments.

## Copilot Troubleshooting
- If suggestions are irrelevant, add more context or comments.
- For issues, consult the [GitHub Copilot documentation](https://docs.github.com/en/copilot).

## Contribution Workflow
- Keep changes minimal and focused; prefer small PRs.
- Update docs and tests alongside code changes.
- Respect warnings-as-errors; build should be clean locally before PR.
- If adding packages, update Directory.Packages.props and evaluate AOT/trimming impact.

---

*Last updated: October 29, 2025*
