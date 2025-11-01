# Architecture Diagram

## High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                          Program                                │
│                     (Entry Point)                               │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ creates
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│              CodeGenerationOrchestrator                         │
│                   (Facade Pattern)                              │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • Coordinates overall generation process                 │  │
│  │ • Manages paths and directories                          │  │
│  │ • Calls factories to create generators                   │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ uses
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│               CodeGeneratorFactory                              │
│                 (Factory Pattern)                               │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ CreateOneOfGenerators()                                  │  │
│  │ CreateResultGenerators()                                 │  │
│  │ CreateGenerators()                                       │  │
│  └──────────────────────────────────────────────────────────┘  │
└───────┬──────────────────────────────┬──────────────────────────┘
        │                              │
        │ creates                      │ creates
        ▼                              ▼
┌──────────────────┐          ┌──────────────────────┐
│ OneOf Generators │          │ Result Generators    │
└──────────────────┘          └──────────────────────┘
        │                              │
        ├──────────────┐              ├──────────────┬──────────────┐
        ▼              ▼              ▼              ▼              ▼
┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐
│ OneOf    │  │ OneOf    │  │ Result   │  │ Result   │  │ Result   │
│ Code Gen │  │ Test Gen │  │ Code Gen │  │ Test Gen │  │ ValueAcc │
└────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘
     │             │             │             │             │
     └─────────────┴─────────────┴─────────────┴─────────────┘
                              │
                              │ extends
                              ▼
                   ┌─────────────────────┐
                   │ BaseCodeGenerator   │
                   │ (Template Method)   │
                   │                     │
                   │ • Generate()        │
                   │ • ValidateInputs()  │
                   │ • PrepareOutputDir()│
                   │ • GenerateForArity()│
                   └─────────────────────┘
```

## Builder Pattern Structure

```
┌──────────────────────────────────────────────────────────────────┐
│         ResultValueAccessExtensionsCodeGenerator                 │
│                                                                  │
│  Injects and uses:                                              │
└──────┬──────────┬──────────┬──────────┬──────────┬─────────────┘
       │          │          │          │          │
       │          │          │          │          │
       ▼          ▼          ▼          ▼          ▼
┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐
│ Value    │ │ To       │ │ ValueOr  │ │ ValueOr  │ │ Async    │
│ Access   │ │ Nullable │ │ Method   │ │ Throw    │ │ Method   │
│ Method   │ │ Method   │ │ Builder  │ │ Method   │ │ Builder  │
│ Builder  │ │ Builder  │ │          │ │ Builder  │ │          │
└──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘
     │            │            │            │            │
     └────────────┴────────────┴────────────┴────────────┘
                           │
                           │ All use
                           ▼
                  ┌─────────────────┐
                  │ GenericType     │
                  │ Helper          │
                  │                 │
                  │ • CreateParams()│
                  │ • BuildString() │
                  │ • BuildTuple()  │
                  └─────────────────┘
```

## Utility Layer

```
┌─────────────────────────────────────────────────────────────┐
│                    Common Utilities                         │
│                   (Shared by All)                           │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌────────────────┐  ┌────────────────┐  ┌──────────────┐ │
│  │ OrdinalHelper  │  │ TestTypeHelper │  │ FileSystem   │ │
│  │                │  │                │  │ Helper       │ │
│  │ GetOrdinal     │  │ GetTestType    │  │              │ │
│  │ Name()         │  │ GetTestValue() │  │ WriteFile()  │ │
│  │                │  │ IsValueTest..()│  │ EnsureDir()  │ │
│  └────────────────┘  └────────────────┘  └──────────────┘ │
│                                                             │
│  ┌────────────────────────────────────────────────────┐    │
│  │ GenericTypeHelper                                  │    │
│  │                                                    │    │
│  │ CreateGenericParameters()                         │    │
│  │ CreateOrdinalGenericParameters()                  │    │
│  │ BuildGenericTypeString()                          │    │
│  │ BuildTupleTypeString()                            │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

## Configuration Layer

```
┌─────────────────────────────────────────────────────────────┐
│                  Configuration                              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────────────────────────────────┐           │
│  │ GenerationConfig                            │           │
│  │ (Options Pattern)                           │           │
│  │                                             │           │
│  │ • BaseNamespace: string                    │           │
│  │ • StartArity: int                          │           │
│  │ • DirectoryName: string                    │           │
│  │ • ClassName: string                        │           │
│  └─────────────────────────────────────────────┘           │
│                                                             │
│  ┌─────────────────────────────────────────────┐           │
│  │ BaseCodeGenerator                           │           │
│  │ (Template Method Pattern)                   │           │
│  │                                             │           │
│  │ + Generate(arity, path)         ◄─────────┐│           │
│  │   ├─ ValidateInputs()                     ││           │
│  │   ├─ PrepareOutputDirectory()             ││           │
│  │   └─ GenerateForArityRange() ◄────────────┼┤           │
│  │                                            ││           │
│  │ # abstract GenerateForArityRange()        ││           │
│  │ # virtual ValidateInputs()                ││           │
│  │ # virtual PrepareOutputDirectory()        ││           │
│  └────────────────────────────────────────────┘│           │
│                                                ││           │
│  Concrete implementations override:            ││           │
│  • OneOfCodeGenerator ─────────────────────────┘│           │
│  • ResultCodeGenerator                          │           │
│  • ResultValueAccessExtensionsCodeGenerator     │           │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

## Data Flow

```
User Input
    │
    │ arity, paths
    ▼
Program
    │
    │ creates
    ▼
CodeGenerationOrchestrator
    │
    ├──► FileSystemHelper.EnsureDirectoryExists()
    │
    ├──► CodeGeneratorFactory.CreateGenerators()
    │         │
    │         └──► Returns: ICodeGenerator[]
    │
    └──► foreach generator:
              │
              └──► generator.Generate(arity, path)
                        │
                        ├──► BaseCodeGenerator.Generate()
                        │         │
                        │         ├──► ValidateInputs()
                        │         ├──► PrepareOutputDirectory()
                        │         └──► GenerateForArityRange() ◄── Overridden
                        │
                        ▼
                   Concrete Generator Implementation
                        │
                        ├──► Uses Builders to create methods
                        │         │
                        │         └──► Builder.Build*Method()
                        │                   │
                        │                   └──► Uses GenericTypeHelper
                        │                   └──► Uses OrdinalHelper
                        │
                        └──► FileSystemHelper.WriteFile()
                                  │
                                  └──► File written to disk
```

## Class Relationships

```
ICodeGenerator ◄─────────────── implemented by ──────────────┐
      ▲                                                       │
      │                                                       │
      │ extends                                               │
      │                                                       │
BaseCodeGenerator                                             │
      ▲                                                       │
      │                                                       │
      │ extends                                               │
      │                 │                    │              │
      ├─────────────────┬────────────────────┐              │
      │                 │                    │              │
OneOfCodeGen    ResultCodeGen      ResultValueAccessGen    │
      │                                      │              │
      │ uses                                 │ uses         │
      │                                      │              │
      ▼                                      ▼              │
OneOfBaseClass                    ┌────────────────┐       │
Builder                           │ Builder        │       │
OneOfImplementation               │ • ValueAccess  │       │
Builder                           │ • ToNullable   │       │
                                  │ • ValueOr      │       │
                                  │ • ValueOrThrow │       │
                                  │ • Async        │       │
                                  └────────────────┘       │
                                           │               │
                                           │ uses          │
                                           ▼               │
                                  ┌────────────────┐       │
                                  │ Common         │       │
                                  │ Utilities      │       │
                                  │                │       │
                                  │ • Ordinal      │       │
                                  │ • TestType     │       │
                                  │ • FileSystem   │       │
                                  │ • GenericType  │       │
                                  └────────────────┘       │
                                                           │
CodeGeneratorFactory ──────────── creates ─────────────────┘
      │
      │ used by
      ▼
CodeGenerationOrchestrator ◄── created by ── Program
```

## Key Design Principles Illustrated

### 1. Single Responsibility
Each box in the diagrams has ONE clear responsibility

### 2. Open/Closed
- BaseCodeGenerator: Open for extension (inheritance), closed for modification
- Builders: Can add new builders without changing existing ones

### 3. Dependency Inversion
- Generators depend on ICodeGenerator abstraction
- Builders injected via constructor (not created internally)

### 4. Composition Over Inheritance
- ResultValueAccessExtensionsCodeGenerator composes 5 builders
- Rather than inheriting complex behavior

### 5. Separation of Concerns
- Utilities: Shared functionality
- Builders: Method construction
- Generators: File orchestration
- Orchestrator: Workflow coordination

## Code Generation Rules

### 1. One Generator Per Class
Each generator should be responsible for generating **only one class**. This ensures clear separation of concerns and maintainable code.

**✅ Correct:**
- `ToNullableExtensionsGenerator` - generates `ToNullableExtensions` class
- `ValueOrExtensionsGenerator` - generates `ValueOrExtensions` class
- `ValueOrThrowExtensionsGenerator` - generates `ValueOrThrowExtensions` class

**❌ Incorrect:**
- `ValueAccessExtensionsGenerator` - generating multiple extension classes in one generator

### 2. RegionFile Generation Structure
When generating a RegionFile, create **one class containing all methods**, with methods organized by **regions based on arity**.

**Structure:**
```csharp
public static class ExtensionClass
{
    #region Arity 2
    // All methods for arity 2
    #endregion

    #region Arity 3
    // All methods for arity 3
    #endregion

    // ... and so on
}
```

This approach:
- Keeps all related methods in a single class
- Provides clear organization by arity
- Makes navigation easier with collapsible regions
- Maintains a single file per generated class
