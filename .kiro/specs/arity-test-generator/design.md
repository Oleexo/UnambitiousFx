# Design Document

## Overview

The Result Direct Methods Test Generator is a simple, focused test generation system integrated within the existing Core.CodeGen project. It generates comprehensive unit tests for Result direct methods (Match, IfSuccess, IfFailure, TryGet) across multiple arities using a straightforward approach that follows the same patterns as existing production code generators.

The generator produces three types of tests:
- **Sync Tests**: For IfSuccess, IfFailure, TryGet (Match is async-only)
- **Task Tests**: For all methods including Match (Task-based async)
- **ValueTask Tests**: For all methods including Match (ValueTask-based async)

The system leverages the existing Core.CodeGen infrastructure directly, following the same patterns as `ResultCodeGenerator` and other production generators. This approach ensures consistency, maintainability, and proper file organization for both SeparateFiles and SingleFile modes.

## Architecture

### Simplified Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    Core.CodeGen Program                         │
│                   (Existing Entry Point)                        │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • Uses existing CodeGenerationOrchestrator                │  │
│  │ • Calls CodeGeneratorFactory.CreateResultTestGenerators  │  │
│  │ • No special flags or routing needed                      │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ uses existing orchestrator
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│            CodeGenerationOrchestrator                           │
│                 (Existing Component)                            │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • GenerateResults() method enhanced                      │  │
│  │ • Calls CreateResultTestGenerators()                     │  │
│  │ • Uses same patterns as production code                   │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ uses enhanced factory
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│              CodeGeneratorFactory                               │
│                (Enhanced Existing Factory)                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ CreateResultGenerators() - existing                      │  │
│  │ CreateResultTestGenerators() - new, simple               │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ creates single generator
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│           ResultDirectMethodsTestsGenerator                     │
│              (Single, Simple Generator)                         │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • Extends BaseCodeGenerator directly                     │  │
│  │ • Generates sync, Task, and ValueTask tests              │  │
│  │ • Uses UnderClass for proper file organization           │  │
│  │ • Follows same patterns as ResultCodeGenerator           │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ extends and uses
                         ▼
                   ┌─────────────────────┐
                   │ BaseCodeGenerator   │
                   │ (Existing Component)│
                   │ • Generate()        │
                   │ • ValidateInputs()  │
                   │ • WriteSingleFile() │
                   │ • WriteSeparated()  │
                   └─────────────────────┘
                              │
                              │ uses existing infrastructure
                              ▼
                   ┌─────────────────────┐
                   │ Core.CodeGen        │
                   │ Shared Components   │
                   │ • ClassWriter       │
                   │ • MethodWriter      │
                   │ • FileWriter        │
                   │ • AttributeReference│
                   │ • FileSystemHelper  │
                   └─────────────────────┘
```

### Test Generation Structure

```
┌──────────────────────────────────────────────────────────────────┐
│           ResultDirectMethodsTestsGenerator                      │
│                                                                  │
│  GenerateForArity(arity) creates:                              │
└──────┬──────────┬──────────┬──────────────────────────────────┘
       │          │          │
       │          │          │
       ▼          ▼          ▼
┌──────────┐ ┌──────────┐ ┌──────────┐
│ Sync     │ │ Task     │ │ ValueTask│
│ Tests    │ │ Tests    │ │ Tests    │
│ Class    │ │ Class    │ │ Class    │
│          │ │          │ │          │
│ • IfSucc │ │ • Match  │ │ • Match  │
│ • IfFail │ │ • IfSucc │ │ • IfSucc │
│ • TryGet │ │ • IfFail │ │ • IfFail │
│          │ │ • TryGet │ │ • TryGet │
└──────────┘ └──────────┘ └──────────┘
     │            │            │
     └────────────┴────────────┘
                  │
                  │ All use simple, direct generation
                  ▼
          ┌─────────────────┐
          │ Simple Methods  │
          │ • GenerateSync  │
          │ • GenerateTask  │
          │ • GenerateVTask │
          │ • GetTestValue  │
          │ • GetTestType   │
          └─────────────────┘
```

## Components and Interfaces

### Single Generator Component

#### ResultDirectMethodsTestsGenerator
```csharp
internal sealed class ResultDirectMethodsTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 1;
    private const string ClassName = "ResultDirectMethodsTests";
    private const string SubNamespace = "Results.Tests";

    private static readonly string[] DirectMethods = { "Match", "IfSuccess", "IfFailure", "TryGet" };

    public ResultDirectMethodsTestsGenerator(string baseNamespace,
                                           FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
        : base(new GenerationConfig(
                   baseNamespace,
                   startArity: StartArity,
                   subNamespace: SubNamespace,
                   className: ClassName,
                   fileOrganization: fileOrganization,
                   isTest: true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        var result = new List<ClassWriter>();

        // Generate sync tests
        var syncTestClass = GenerateSyncTests(arity);
        if (syncTestClass != null) result.Add(syncTestClass);

        // Generate Task async tests
        var taskTestClass = GenerateTaskTests(arity);
        if (taskTestClass != null)
        {
            taskTestClass.UnderClass = ClassName; // Ensures proper file organization
            result.Add(taskTestClass);
        }

        // Generate ValueTask async tests
        var valueTaskTestClass = GenerateValueTaskTests(arity);
        if (valueTaskTestClass != null)
        {
            valueTaskTestClass.UnderClass = ClassName; // Ensures proper file organization
            result.Add(valueTaskTestClass);
        }

        return result;
    }
}
```

### Simple Test Generation Methods

#### Test Class Generation
```csharp
private ClassWriter? GenerateSyncTests(ushort arity)
{
    var className = $"ResultDirectMethodsSyncTestsArity{arity}";
    var classWriter = new ClassWriter(className, Visibility.Public)
    {
        Region = $"Arity {arity} - Sync Tests"
    };

    var hasTests = false;

    // Generate sync tests for IfSuccess, IfFailure, TryGet (Match is async-only)
    foreach (var method in DirectMethods.Where(m => m != "Match"))
    {
        var testMethods = GenerateSyncTestMethods(method, arity);
        foreach (var testMethod in testMethods)
        {
            classWriter.AddMethod(testMethod);
            hasTests = true;
        }
    }

    return hasTests ? classWriter : null;
}
```

#### Test Method Generation
```csharp
private MethodWriter GenerateSyncSuccessTest(string methodName, ushort arity)
{
    var testName = $"{methodName}_Arity{arity}_Success_ShouldExecuteCorrectly";
    var body = GenerateSyncSuccessTestBody(methodName, arity);

    return new MethodWriter(
        name: testName,
        returnType: "void",
        body: body,
        visibility: Visibility.Public,
        attributes: new[] { new AttributeReference("Fact") },
        usings: GetRequiredUsings()
    );
}
```

#### Test Body Generation with Gherkin Structure
```csharp
private string GenerateSyncSuccessTestBody(string methodName, ushort arity)
{
    var testValues = GenerateTestValues(arity);
    var resultCreation = GenerateResultCreation(arity, testValues, true);
    var methodCall = GenerateSyncMethodCall(methodName, arity);
    var assertions = GenerateSuccessAssertions(methodName, arity);

    return $@"// Given: A successful Result with test values
{resultCreation}

// When: Calling {methodName}
{methodCall}

// Then: Should execute successfully
{assertions}";
}
```

## Integration with Existing Core.CodeGen

### Seamless Integration Approach

The Result Direct Methods Test Generator integrates seamlessly with existing Core.CodeGen infrastructure using the same patterns as production generators:

#### Reused Components (100% Existing)
- **Design Layer**: `ClassWriter`, `MethodWriter`, `FileWriter`, `AttributeReference`
- **Common Utilities**: `FileSystemHelper` (enhanced for directory creation)
- **Configuration**: `GenerationConfig` base class, `BaseCodeGenerator` template method pattern
- **Base Interfaces**: `ICodeGenerator` interface for consistency

#### No New Infrastructure Required
- **No Custom Orchestration**: Uses existing `CodeGenerationOrchestrator`
- **No Custom Configuration**: Uses standard `GenerationConfig`
- **No Custom Interfaces**: Implements existing `ICodeGenerator`
- **No Custom Utilities**: Uses simple, direct generation methods

#### Integration Points

##### Existing Program Entry Point (No Changes)
```csharp
// Program.cs remains unchanged
public static void Main(string[] args)
{
    var orchestrator = new CodeGenerationOrchestrator(
        baseNamespace: Constant.BaseNamespace,
        sourceDirectory: TargetSourceDirectory,
        testDirectory: TargetTestDirectory,
        targetArity: TargetArity,
        fileOrganization: FileOrganization
    );
    orchestrator.Execute();
}
```

##### Enhanced Factory (Minimal Addition)
```csharp
// CodeGeneratorFactory.cs - simple addition
public static class CodeGeneratorFactory
{
    // Existing methods unchanged...
    
    public static IEnumerable<ICodeGenerator> CreateResultTestGenerators(
        string baseNamespace,
        FileOrganizationMode fileOrganization = FileOrganizationMode.SeparateFiles)
    {
        // Single, simple generator
        yield return new ResultDirectMethodsTestsGenerator(baseNamespace, fileOrganization);
    }
}
```

##### Enhanced Orchestrator (Minimal Addition)
```csharp
// CodeGenerationOrchestrator.cs - one method enhanced
private void GenerateResults(string sourceDirectoryPath, string testDirectoryPath)
{
    // Existing production code generation...
    foreach (var generator in CodeGeneratorFactory.CreateResultGenerators(_baseNamespace, _fileOrganization))
    {
        generator.Generate(_targetArity, sourceDirectoryPath);
    }

    // New test code generation (simple addition)
    foreach (var generator in CodeGeneratorFactory.CreateResultTestGenerators(_baseNamespace, _fileOrganization))
    {
        generator.Generate(_targetArity, testDirectoryPath);
    }
}
```

### Directory Structure (Minimal Impact)

```
src/Core.CodeGen/
├── TestBuilders/
│   ├── OneOf/                              # Existing
│   └── Results/
│       ├── ResultDirectMethodsTestsGenerator.cs  # New - Single file
│       └── [existing files]               # Existing Result test builders
├── [all existing directories unchanged]
└── [all existing files unchanged]
```

This structure adds only one new file while leveraging all existing infrastructure.

## Method Coverage Matrix

### Result Direct Methods (Current Implementation)
These methods are implemented directly on the Result class and are covered by the current generator:

| Method | Sync | Task Async | ValueTask Async | Special Notes |
|--------|------|------------|-----------------|---------------|
| Match | ❌ | ✅ | ✅ | Async-only method |
| IfSuccess | ✅ | ✅ | ✅ | |
| IfFailure | ✅ | ✅ | ✅ | |
| TryGet | ✅ | ✅ | ✅ | |

### Test Generation Strategy
Each method is tested across:
- **All supported arities**: Result<T>, Result<T1, T2>, Result<T1, T2, T3>, Result<T1, T2, T3, T4>, Result<T1, T2, T3, T4, T5>
- **Two test scenarios per method**: Success and Failure cases
- **Three test class types**:
  - **Sync Tests**: For IfSuccess, IfFailure, TryGet (Match excluded as it's async-only)
  - **Task Tests**: For all methods including Match
  - **ValueTask Tests**: For all methods including Match

### Generated Test Structure
For each arity, three test classes are generated:

```csharp
// Sync Tests (Match excluded)
public class ResultDirectMethodsSyncTestsArity1
{
    [Fact] public void IfSuccess_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public void IfSuccess_Arity1_Failure_ShouldHandleCorrectly() { ... }
    [Fact] public void IfFailure_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public void IfFailure_Arity1_Failure_ShouldHandleCorrectly() { ... }
    [Fact] public void TryGet_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public void TryGet_Arity1_Failure_ShouldHandleCorrectly() { ... }
}

// Task Tests (All methods)
public class ResultDirectMethodsTaskTestsArity1
{
    [Fact] public async Task MatchTask_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public async Task MatchTask_Arity1_Failure_ShouldHandleCorrectly() { ... }
    [Fact] public async Task IfSuccessTask_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public async Task IfSuccessTask_Arity1_Failure_ShouldHandleCorrectly() { ... }
    // ... etc for IfFailure and TryGet
}

// ValueTask Tests (All methods)
public class ResultDirectMethodsValueTaskTestsArity1
{
    [Fact] public async ValueTask MatchValueTask_Arity1_Success_ShouldExecuteCorrectly() { ... }
    [Fact] public async ValueTask MatchValueTask_Arity1_Failure_ShouldHandleCorrectly() { ... }
    // ... etc for all methods
}
```

## Data Models

### Simple Test Generation (No Complex Models)

The simplified design eliminates complex data models in favor of direct, simple generation methods:

#### Test Value Generation
```csharp
private string GetTestValue(int index)
{
    return index switch
    {
        1 => "42",
        2 => "\"test\"",
        3 => "true",
        4 => "3.14",
        5 => "123L",
        _ => $"\"value{index}\""
    };
}

private string GetTestType(int index)
{
    return index switch
    {
        1 => "int",
        2 => "string",
        3 => "bool",
        4 => "double",
        5 => "long",
        _ => "string"
    };
}
```

#### Test Body Generation
```csharp
// Simple string-based test body generation with Gherkin structure
private string GenerateSyncSuccessTestBody(string methodName, ushort arity)
{
    var testValues = GenerateTestValues(arity);
    var resultCreation = GenerateResultCreation(arity, testValues, true);
    var methodCall = GenerateSyncMethodCall(methodName, arity);
    var assertions = GenerateSuccessAssertions(methodName, arity);

    return $@"// Given: A successful Result with test values
{resultCreation}

// When: Calling {methodName}
{methodCall}

// Then: Should execute successfully
{assertions}";
}
```

#### File Organization Models
Uses existing `FileOrganizationMode` enum:
- **SeparateFiles**: Each arity gets separate files
- **SingleFile**: All arities in one file with regions (using `UnderClass` property)

## Error Handling

### Simplified Error Handling

The simplified design leverages existing BaseCodeGenerator error handling with minimal additions:

#### Existing Error Handling (Inherited)
- **Input Validation**: BaseCodeGenerator validates arity ranges and output paths
- **File System Operations**: Enhanced FileSystemHelper ensures directory creation
- **Generation Failures**: BaseCodeGenerator handles generation errors gracefully

#### Enhanced File System Handling
```csharp
// FileSystemHelper.WriteFile enhanced to create directories
public static void WriteFile(FileWriter fileWriter, string filePath)
{
    // Ensure the directory exists before writing the file
    var directory = Path.GetDirectoryName(filePath);
    if (!string.IsNullOrEmpty(directory))
    {
        EnsureDirectoryExists(directory);
    }

    // Existing file writing logic...
}
```

#### Simple Error Recovery
- **Continue on Failure**: If one test class fails to generate, others continue
- **Null Handling**: Methods return null for failed generations, which are filtered out
- **Exception Logging**: Simple console logging for debugging generation issues

#### No Complex Error Handling Required
- **No Configuration Validation**: Uses standard GenerationConfig validation
- **No Pattern Analysis**: No complex pattern analysis to fail
- **No Retry Logic**: Simple, direct generation doesn't require retries
- **No Cleanup**: BaseCodeGenerator handles cleanup automatically

## Testing Strategy

### Simplified Testing Approach

The simplified design requires minimal testing due to its straightforward nature:

#### Generator Testing
Simple unit tests for the generator itself:

```csharp
[Fact]
public void ResultDirectMethodsTestsGenerator_ShouldCreateCorrectNumberOfClasses()
{
    // Given: A test generator
    var generator = new ResultDirectMethodsTestsGenerator("UnambitiousFx.Core");
    
    // When: Generating for arity 2
    var classes = generator.GenerateForArity(2);
    
    // Then: Should create 3 classes (Sync, Task, ValueTask)
    Assert.Equal(3, classes.Count);
    Assert.Contains(classes, c => c.Name.Contains("Sync"));
    Assert.Contains(classes, c => c.Name.Contains("Task"));
    Assert.Contains(classes, c => c.Name.Contains("ValueTask"));
}
```

#### Generated Test Validation
Basic validation that generated tests compile and run:

```csharp
[Fact]
public void GeneratedTests_ShouldCompileAndExecute()
{
    // Given: Generated test files
    var generator = new ResultDirectMethodsTestsGenerator("UnambitiousFx.Core");
    
    // When: Generating tests for arity 1
    generator.Generate(1, "test/output");
    
    // Then: Generated files should exist and compile
    Assert.True(File.Exists("test/output/Results/Tests/ResultDirectMethodsTests.1.g.cs"));
    // Additional compilation and execution validation...
}
```

#### Integration Testing
- **File Organization Tests**: Verify SeparateFiles vs SingleFile modes work correctly
- **Directory Creation Tests**: Ensure proper directory structure is created
- **Cross-Arity Tests**: Validate generation works for different arities

#### Quality Assurance

#### Simple Quality Measures
- **Compilation Verification**: Generated tests must compile successfully
- **Execution Verification**: Generated tests must run without errors
- **Naming Consistency**: Test names follow consistent patterns
- **Gherkin Structure**: Generated test bodies contain Given-When-Then comments

#### Benefits of Simplified Approach
- **Fewer Tests Required**: Simple generator requires fewer tests
- **Easier Maintenance**: Less complex testing infrastructure to maintain
- **Faster Feedback**: Simple tests run quickly and provide clear results
- **Lower Risk**: Straightforward implementation has fewer failure modes

The simplified testing strategy focuses on essential validation while avoiding the complexity of testing a complex, multi-component system.