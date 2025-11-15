UnambitiousFx – Development Guidelines

Audience: Senior .NET developers contributing to UnambitiousFx. Keep changes minimal, consistent with the repo’s conventions, and AOT-friendly.

1) Build and Configuration Notes
- SDK selection
  - global.json only constrains allowPrerelease: false. Use a stable .NET SDK (9.x recommended, as most projects target net9.0; Mediator.Generator targets netstandard2.0).
  - Ensure your local SDK includes Roslyn analyzers and SourceLink support (standard in modern SDKs).
- Centralized MSBuild configuration
  - Root build.props (imported by src/test/examples Directory.Build.props) defines:
    - WarningsAsErrors = true and TreatWarningsAsErrors = true — zero-warning policy.
    - Nullable = enable; ImplicitUsings = enable; LangVersion = latest.
    - AssemblyName/RootNamespace derived from Product + project name.
  - src/Directory.Build.props
    - IsAotCompatible = true for all src libraries; keep APIs AOT-friendly (no runtime codegen, avoid reflection-heavy patterns unless trimmed and annotated).
    - Packaging metadata (RepositoryUrl, PackageProjectUrl, symbols, snupkg, GenerateDocumentationFile = true).
    - PackageOutputPath = $(SolutionDir)local_packages (local pack target output when GeneratePackageOnBuild is enabled or pack is invoked manually).
    - Adds Microsoft.SourceLink.GitHub only under CI to avoid local build noise.
    - Ensures README.md is included in packages (copied from solution root if not present at project level).
  - test/Directory.Build.props
    - TargetFramework = net9.0, IsPackable = false, IsTestProject = true.
    - Adds Using Include="Xunit" and standard test packages via central package management.
- Central Package Management (Directory.Packages.props)
  - Versions are managed centrally (BenchmarkDotNet, xunit, coverlet, JetBrains.Annotations, MediatR, Microsoft.* abstractions/options/logging, Roslyn analyzers, etc.).
  - Do not pin versions in individual csproj files unless there is a strong justification; prefer updating Directory.Packages.props.
- Projects and targets
  - Core (net9.0): Result, OneOf, Options primitives and extensions.
  - Core.XUnit (net9.0): test helpers/assertions for this repo.
  - Core.CodeGen (net9.0): internal code generation utilities used to emit result/oneof types and tests in output/ when needed. Do not assume it runs as a source generator.
  - Mediator (net9.0) and Mediator.Abstractions (net9.0): mediator runtime.
  - Mediator.Generator (netstandard2.0): Roslyn source generator for mediator handlers/resolvers.
  - Tests under test/* (net9.0) and examples/* contain example apps (including WebApi and WebApiAot projects).
- Build outputs and packing
  - Symbols are produced as .snupkg when packing. GeneratePackageOnBuild is false by default; invoke dotnet pack explicitly if needed.
  - Local package output goes to local_packages at solution root per src/Directory.Build.props.
- AOT/Trimming
  - Favor code paths that work under trimming. Avoid unbounded reflection, dynamic assembly loading, and runtime IL generation. If reflection is required, annotate with DynamicDependency/DynamicallyAccessedMembers where appropriate and keep surface small.

2) Development Conventions and Code Style
- Language and compiler configuration
  - Nullable enabled: avoid #nullable disable; model nullability accurately.
  - LangVersion latest: modern features acceptable when they don’t hurt AOT or public API stability.
  - Warnings as errors: fix or suppress with justification; prefer code fixes over pragmas.
- API design
  - Maintain immutability of value-like types; prefer records for DTOs/events/requests where appropriate (see examples/ and docs guidance).
  - Keep public surface minimal and coherent; avoid leaking implementation details or generator-only constructs.
  - Favor explicit Result and OneOf return types to model success/failure and variant outcomes instead of exceptions for domain flow.
- Error/Reason model
  - BaseResult tracks Reasons (ISuccess/IError) and Metadata (case-insensitive keys). Failure debugger display prioritizes domain errors over ExceptionalError; keep that behavior intact.
  - When adding new error types, provide meaningful Code and Message; ensure they’re AOT-safe and serializable if used over boundaries.
- Collections/allocations
  - Use ReadOnly collections for exposures (AsReadOnly, ToArray) as in BaseResult. Be mindful of per-call allocations on hot paths; consider caching or avoiding ToArray if a path is hot.
- Source generation and codegen
  - Core.CodeGen contains ResultCodeGenerator and OneOfTestsGenerator. These are internal utilities to generate families of types/tests. If you modify their output shape, update dependent tests and docs.
  - Mediator.Generator (netstandard2.0) must stay compatible with multiple TFMs; avoid APIs newer than C# 9 runtime assumptions inside the generator. Keep analyzer and generator diagnostics precise.
- Testing organization
  - Tests are split under test/Core.Tests, test/Core.XUnit.Tests, and test/Mediator.Tests. The repo uses xUnit with centralized includes and coverlet for coverage collection.
  - Core.XUnit provides custom assertions/helpers; prefer them where appropriate to keep test style consistent.
- Documentation
  - docs/docs and docs/_site provide user docs; keep docs in sync with code-level behaviors, especially around Result semantics, Mediator patterns, and AOT guidance.

3) C# Best Practices (tailored) and Complexity Rules
- General
  - Prefer pure functions and side-effect minimization; thread-safety matters for mediator and result flows.
  - Use sealed classes for concrete types unless inheritance is intended; improves inlining and reduces vtable cost.
  - Consider struct (readonly struct/record struct) for small, immutable, value-like types used in high-frequency paths; measure with benchmarks before committing.
- Exceptions vs Results
  - Reserve exceptions for truly exceptional conditions. Use Result and IError for domain/control flow. Ensure Match/IfSuccess/IfFailure paths are symmetric and allocation-conscious.
- LINQ and performance
  - LINQ is acceptable for clarity; for hot paths or allocations-sensitive areas, prefer loops or Span-based approaches. Avoid multiple enumerations unless cached.
- Allocation and copying
  - Avoid defensive copies of large collections; expose IReadOnly interfaces and document ownership. Use AsReadOnly or ReadOnlySpan where applicable.
- AOT/Trimming best practices
  - Avoid reflection on open generics or hidden members; when unavoidable, constrain with generic type parameters and annotate with [DynamicallyAccessedMembers] or DynamicDependency.
  - Keep expression trees out of runtime paths; if used, ensure they’re compiled at build-time or guarded for AOT.
- Concurrency
  - Use async-friendly patterns; avoid sync-over-async. In Mediator pipelines and publishers, prefer cancellation tokens propagation and bounded parallelism when dispatching.
- Code complexity rules
  - Cyclomatic complexity: keep methods <= 10 for typical business logic, <= 15 for core library internals; split into smaller methods when exceeding. Justify and document exceptions (e.g., generated code or performance-critical hot paths with early exits).
  - Cognitive complexity: target <= 15. Refactor nested conditionals into guard clauses and polymorphism. Prefer Match/Map/Tap combinators for Result flows over deeply nested if/else.
  - Method length: prefer < 50 lines; for orchestrator methods, < 80 with clear regions/sections.
  - Parameter count: prefer <= 5. Use parameter objects or records for larger sets. Maintain strict generic arity only where library design demands (Result1..Result8 are intentional).
  - Coupling: keep classes focused (single responsibility). For Mediator, split request/handler concerns; for Results, extend via extension methods rather than modifying core types unless necessary.
  - Nullability: no unguarded null dereference; avoid using ! (null-forgiving) unless absolutely certain and documented.
- Style specifics
  - Naming: PascalCase for public types/members; camelCase for locals/parameters; _prefix for private fields.
  - Using directives: rely on ImplicitUsings; add explicit usings only when necessary. Keep file-scoped namespaces consistent with the project style.
  - Partial classes: used in Result and related types; when extending, keep related members grouped and avoid cross-file hidden coupling.
  - Analyzers: honor Microsoft.CodeAnalysis.Analyzers guidance; address warnings rather than suppressing.

4) Working with Benchmarks and Examples
- Benchmarks (benchmarks/*)
  - Use BenchmarkDotNet projects to measure changes (e.g., Result vs FluentResults). Keep benchmark code isolated from runtime code and avoid pulling benchmark-only dependencies into src/.
  - When performance changes are expected, update or add benchmarks; justify regressions or trade-offs.
- Examples (examples/mediator)
  - Example WebApi and WebApiAot projects illustrate usage. Maintain AOT compatibility and simple DI registration patterns. Keep examples aligned with documented practices.

5) Contribution Workflow Tips
- Keep changes minimal and focused; prefer small PRs with clear purpose.
- Update docs and tests alongside code changes.
- Respect warnings-as-errors; build should be clean locally before PR (CI enforces this).
- If adding packages, update Directory.Packages.props and evaluate AOT/trimming impact.
- For source generator changes, test across netstandard2.0 host and ensure compatibility with net9.0 consumers.

6) Quick Reference
- Targets: src projects net9.0 (except Mediator.Generator netstandard2.0); tests net9.0.
- Central props: build.props, src/test Directory.Build.props, Directory.Packages.props.
- AOT: IsAotCompatible=true in src. Avoid reflection-heavy features.
- Packaging: pack to local_packages; symbols snupkg; README included.
- Results/OneOf: prefer composable APIs and extension methods; keep symmetry in Match/Map/Tap methods.
