# Design Document

## Overview

The Arity Test Generator is a standalone system designed to generate comprehensive unit tests for Result extension methods across multiple arities. Following the existing code generator architecture as a reference, this system implements its own distinct architecture with clear separation of concerns. The generator produces focused, single-responsibility tests using Gherkin Given-When-Then structure to ensure maintainability and clarity.

The system operates independently from the main code generator while leveraging similar organizational patterns. It analyzes existing Result test patterns in the Core.Tests project and generates tests that follow the same conventions, naming patterns, and assertion libraries.

## Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      TestProgram                                │
│                   (Entry Point)                                 │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ creates
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│            ArityTestGenerationOrchestrator                      │
│                 (Facade Pattern)                                │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ • Coordinates test generation process                    │  │
│  │ • Manages test output directories                        │  │
│  │ • Calls factories to create test generators              │  │
│  │ • Analyzes existing test patterns                        │  │
│  └──────────────────────────────────────────────────────────┘  │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         │ uses
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│              ArityTestGeneratorFactory                          │
│                (Factory Pattern)                                │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │ CreateResultTestGenerators()                             │  │
│  │ CreateAsyncResultTestGenerators()                        │  │
│  │ CreateErrorHandlingTestGenerators()                      │  │
│  └──────────────────────────────────────────────────────────┘  │
└───────┬──────────────────────────────┬──────────────────────────┘
        │                              │
        │ creates                      │ creates
        ▼                              ▼
┌──────────────────┐          ┌──────────────────────┐
│ Result Test      │          │ Async Result Test    │
│ Generators       │          │ Generators           │
└──────────────────┘          └──────────────────────┘
        │                              │
        ├──────────────┬──────────────┬┴──────────────┬──────────────┐
        ▼              ▼              ▼              ▼              ▼
┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐
│ Value    │  │ Transform│  │ Error    │  │ Async    │  │ Validation│
│ Access   │  │ Test Gen │  │ Handling │  │ Test Gen │  │ Test Gen │
│ Test Gen │  │          │  │ Test Gen │  │          │  │          │
└────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘  └────┬─────┘
     │             │             │             │             │
     └─────────────┴─────────────┴─────────────┴─────────────┘
                              │
                              │ extends
                              ▼
                   ┌─────────────────────┐
                   │ BaseArityTestGen    │
                   │ (Template Method)   │
                   │                     │
                   │ • Generate()        │
                   │ • ValidateInputs()  │
                   │ • PrepareOutputDir()│
                   │ • GenerateForArity()│
                   └─────────────────────┘
```

### Test Builder Pattern Structure

```
┌──────────────────────────────────────────────────────────────────┐
│              ResultValueAccessTestGenerator                      │
│                                                                  │
│  Injects and uses:                                              │
└──────┬──────────┬──────────┬──────────┬──────────┬─────────────┘
       │          │          │          │          │
       │          │          │          │          │
       ▼          ▼          ▼          ▼          ▼
┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐
│ Value    │ │ To       │ │ ValueOr  │ │ ValueOr  │ │ Match    │
│ Test     │ │ Nullable │ │ Test     │ │ Throw    │ │ Test     │
│ Builder  │ │ Test     │ │ Builder  │ │ Test     │ │ Builder  │
│          │ │ Builder  │ │          │ │ Builder  │ │          │
└──────────┘ └──────────┘ └──────────┘ └──────────┘ └──────────┘
     │            │            │            │            │
     └────────────┴────────────┴────────────┴────────────┘
                           │
                           │ All use
                           ▼
                  ┌─────────────────┐
                  │ Test Utilities  │
                  │                 │
                  │ • GherkinTest   │
                  │ • TestData      │
                  │ • Assertion     │
                  │ • Pattern       │
                  └─────────────────┘
```

## Components and Interfaces

### Core Interfaces

#### IArityTestGenerator
```csharp
internal interface IArityTestGenerator
{
    void Generate(ushort maxArity, string outputPath);
    string GetTestClassName();
    IEnumerable<string> GetRequiredUsings();
}
```

#### ITestPatternAnalyzer
```csharp
internal interface ITestPatternAnalyzer
{
    TestPattern AnalyzeExistingTests(string testDirectory);
    NamingConvention ExtractNamingConvention();
    AssertionPattern ExtractAssertionPattern();
}
```

#### IGherkinTestBuilder
```csharp
internal interface IGherkinTestBuilder
{
    TestMethod BuildGherkinTest(string methodName, ushort arity, TestScenario scenario);
    string BuildGivenSection(TestData testData);
    string BuildWhenSection(string methodCall);
    string BuildThenSection(string expectedResult);
}
```

### Configuration Components

#### ArityTestGenerationConfig
```csharp
internal sealed class ArityTestGenerationConfig
{
    public string BaseNamespace { get; init; }
    public ushort MinArity { get; init; } = 1;
    public ushort MaxArity { get; init; }
    public string TestDirectory { get; init; }
    public bool GenerateAsyncTests { get; init; } = true;
    public bool GenerateErrorHandlingTests { get; init; } = true;
    public TestNamingPattern NamingPattern { get; init; }
    public IEnumerable<TestScenarioCategory> EnabledScenarios { get; init; }
}
```

#### TestScenario
```csharp
internal sealed class TestScenario
{
    public string Name { get; init; }
    public TestScenarioType Type { get; init; }
    public ushort Arity { get; init; }
    public string MethodName { get; init; }
    public TestData InputData { get; init; }
    public ExpectedResult ExpectedResult { get; init; }
}
```

### Test Builder Components

#### GherkinTestMethodBuilder
```csharp
internal sealed class GherkinTestMethodBuilder : IGherkinTestBuilder
{
    public TestMethod BuildGherkinTest(string methodName, ushort arity, TestScenario scenario)
    {
        var givenSection = BuildGivenSection(scenario.InputData);
        var whenSection = BuildWhenSection(scenario.MethodCall);
        var thenSection = BuildThenSection(scenario.ExpectedResult);
        
        return new TestMethod
        {
            Name = GenerateTestName(methodName, arity, scenario),
            Body = CombineGherkinSections(givenSection, whenSection, thenSection),
            Attributes = GenerateTestAttributes(scenario),
            Usings = GetRequiredUsings()
        };
    }
}
```

#### ResultTestDataGenerator
```csharp
internal sealed class ResultTestDataGenerator
{
    public TestData GenerateSuccessData(ushort arity)
    public TestData GenerateFailureData(ushort arity)
    public TestData GenerateExceptionData(ushort arity)
    public TestData GenerateEdgeCaseData(ushort arity)
    public TestData GenerateAsyncData(ushort arity)
}
```

#### ResultAssertionGenerator
```csharp
internal sealed class ResultAssertionGenerator
{
    public string GenerateSuccessAssertion(TestScenario scenario)
    public string GenerateFailureAssertion(TestScenario scenario)
    public string GenerateExceptionAssertion(TestScenario scenario)
    public string GenerateAsyncAssertion(TestScenario scenario)
}
```

## Data Models

### Test Pattern Analysis

#### TestPattern
```csharp
internal sealed class TestPattern
{
    public NamingConvention NamingConvention { get; init; }
    public AssertionPattern AssertionPattern { get; init; }
    public IEnumerable<string> CommonUsings { get; init; }
    public IEnumerable<AttributePattern> AttributePatterns { get; init; }
    public DirectoryStructure DirectoryStructure { get; init; }
}
```

#### NamingConvention
```csharp
internal sealed class NamingConvention
{
    public string TestClassSuffix { get; init; } = "Tests";
    public string TestMethodPattern { get; init; }
    public string SuccessTestSuffix { get; init; }
    public string FailureTestSuffix { get; init; }
    public string ExceptionTestSuffix { get; init; }
    public string AsyncTestSuffix { get; init; }
}
```

### Test Generation Models

#### TestMethod
```csharp
internal sealed class TestMethod
{
    public string Name { get; init; }
    public string Body { get; init; }
    public IEnumerable<string> Attributes { get; init; }
    public IEnumerable<string> Usings { get; init; }
    public IEnumerable<MethodParameter> Parameters { get; init; }
    public string ReturnType { get; init; } = "void";
}
```

#### TestData
```csharp
internal sealed class TestData
{
    public ushort Arity { get; init; }
    public IEnumerable<TypeValuePair> Values { get; init; }
    public ResultState ExpectedState { get; init; }
    public string SetupCode { get; init; }
}
```

#### ExpectedResult
```csharp
internal sealed class ExpectedResult
{
    public ResultState State { get; init; }
    public string AssertionCode { get; init; }
    public IEnumerable<string> ValidationChecks { get; init; }
}
```

## Error Handling

### Test Generation Error Handling

The system implements comprehensive error handling for test generation scenarios:

#### Configuration Validation
- Validates arity ranges (min ≤ max, both > 0)
- Ensures output directories are accessible
- Validates naming pattern syntax
- Checks for conflicting scenario configurations

#### Pattern Analysis Error Handling
- Handles missing or inaccessible test directories
- Provides fallback patterns when analysis fails
- Logs warnings for inconsistent existing patterns
- Gracefully handles partial pattern extraction

#### Test Generation Error Handling
- Validates test data generation for each arity
- Handles type resolution failures gracefully
- Provides meaningful error messages for generation failures
- Implements retry logic for transient file system issues

#### File System Error Handling
- Ensures directory creation with proper permissions
- Handles file write conflicts and locks
- Provides atomic file operations where possible
- Implements cleanup on generation failures

### Error Recovery Strategies

#### Graceful Degradation
- Falls back to basic test patterns when advanced analysis fails
- Generates simplified tests when complex scenarios fail
- Continues generation for successful arities when others fail
- Provides partial results with clear error reporting

#### Validation and Feedback
- Pre-validates all inputs before generation starts
- Provides detailed error messages with suggested fixes
- Logs progress and issues for debugging
- Generates summary reports of successful and failed operations

## Testing Strategy

### Unit Testing Approach

#### Test Generator Testing
Each test generator component will have focused unit tests:

- **Pattern Analysis Tests**: Verify correct extraction of naming conventions, assertion patterns, and directory structures from existing test files
- **Test Builder Tests**: Validate Gherkin test structure generation, proper Given-When-Then organization, and correct assertion generation
- **Data Generation Tests**: Ensure appropriate test data creation for different Result arities, types, and scenarios
- **Configuration Tests**: Verify proper validation and handling of generation configuration options

#### Gherkin Structure Testing
Tests for the test generators themselves will follow Gherkin structure:

```csharp
[Fact]
public void GenerateValueAccessTest_ForArity2_ShouldCreateGherkinStructuredTest()
{
    // Given: A Result<int, string> test scenario
    var scenario = new TestScenario
    {
        Arity = 2,
        MethodName = "Value",
        Type = TestScenarioType.Success
    };
    
    // When: Generating the test method
    var testMethod = _gherkinBuilder.BuildGherkinTest("Value", 2, scenario);
    
    // Then: The generated test should have proper Gherkin structure
    Assert.Contains("// Given:", testMethod.Body);
    Assert.Contains("// When:", testMethod.Body);
    Assert.Contains("// Then:", testMethod.Body);
    Assert.Equal("Value_ForArity2Success_ShouldReturnExpectedValue", testMethod.Name);
}
```

#### Integration Testing
- **End-to-End Generation Tests**: Verify complete test generation workflow from configuration to file output
- **Pattern Compatibility Tests**: Ensure generated tests are compatible with existing test infrastructure
- **Cross-Arity Tests**: Validate consistent behavior across different arity ranges
- **File System Integration Tests**: Test actual file generation, directory creation, and cleanup

#### Test Data Strategy
- **Parameterized Tests**: Use theory/inline data for testing multiple arity combinations
- **Mock Dependencies**: Mock file system operations for unit tests, use real file system for integration tests
- **Test Fixtures**: Create reusable test data sets for common Result scenarios
- **Assertion Helpers**: Build custom assertion methods for validating generated test code structure

### Quality Assurance

#### Code Quality Measures
- **Static Analysis**: Ensure generated test code passes static analysis rules
- **Compilation Verification**: Validate that all generated tests compile successfully
- **Naming Convention Compliance**: Verify generated tests follow established naming patterns
- **Performance Testing**: Ensure test generation completes within reasonable time limits

#### Generated Test Validation
- **Syntax Validation**: Ensure generated test code has correct C# syntax
- **Semantic Validation**: Verify generated tests actually test the intended functionality
- **Coverage Analysis**: Confirm generated tests provide comprehensive coverage of Result methods
- **Execution Validation**: Run generated tests to ensure they execute successfully

The testing strategy ensures both the test generator itself and the tests it generates are reliable, maintainable, and provide comprehensive coverage of Result functionality across all supported arities.