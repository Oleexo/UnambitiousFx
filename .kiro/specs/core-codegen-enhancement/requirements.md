# Requirements Document

## Introduction

The Core.CodeGen project is a code generation system that automatically creates C# classes and extension methods with multiple arities to avoid manual repetitive coding. The Core library already contains manually written extension methods for error handling, async operations, and unit tests. This enhancement will create generators that can analyze the existing manually written code and automatically generate the corresponding arity-based versions, eliminating the need to manually write repetitive code for different arities.

## Glossary

- **Core.CodeGen**: The code generation project that creates C# source files
- **Arity**: The number of generic type parameters (e.g., Result<T1, T2> has arity 2)
- **Generator**: A class implementing ICodeGenerator that produces specific types of code
- **Builder**: A helper class that constructs method implementations for generators
- **Extension Methods**: Static methods that extend existing types with additional functionality
- **Design System**: The framework within Core.CodeGen for programmatically writing C# code
- **Error Handling Extensions**: Methods for managing and transforming error states in Result types
- **Async Extensions**: Methods for handling asynchronous Result operations
- **Unit Tests**: Automated tests that verify the correctness of generated code

## Requirements

### Requirement 1

**User Story:** As a developer maintaining the Core library, I want generators that can analyze existing error handling extension methods and generate arity-based versions, so that I don't have to manually write repetitive code for different Result arities.

#### Acceptance Criteria

1. WHEN the code generation runs, THE Core.CodeGen SHALL analyze existing error handling extension methods in src/Core/Results/Extensions/ErrorHandling
2. THE Core.CodeGen SHALL generate arity-based versions of MapError, MapErrors, FilterError, HasError, HasException, and Recovery methods
3. THE Core.CodeGen SHALL generate arity-based versions of AppendError, PrependError, and Accumulate methods
4. THE Core.CodeGen SHALL generate arity-based versions of FindError and MatchError methods
5. THE Core.CodeGen SHALL generate arity-based versions of ShapeError methods



### Requirement 3

**User Story:** As a developer maintaining the Core library, I want generators that can analyze existing async extension methods and generate arity-based versions, so that async Result operations work consistently across all arities.

#### Acceptance Criteria

1. WHEN the code generation runs, THE Core.CodeGen SHALL analyze existing async extension methods in Tasks and ValueTasks subdirectories
2. THE Core.CodeGen SHALL generate arity-based Task versions of transformation methods found in existing code
3. THE Core.CodeGen SHALL generate arity-based ValueTask versions of transformation methods found in existing code
4. THE Core.CodeGen SHALL generate arity-based async versions of validation methods found in existing code
5. THE Core.CodeGen SHALL generate arity-based async versions of side-effect methods found in existing code

### Requirement 4

**User Story:** As a developer maintaining the Core library, I want generators that can analyze existing unit tests and generate arity-based test versions, so that all generated code has corresponding test coverage without manual duplication.

#### Acceptance Criteria

1. WHEN the code generation runs, THE Core.CodeGen SHALL analyze existing unit tests in test/Core.Tests for extension methods
2. THE Core.CodeGen SHALL generate arity-based unit tests for all generated Result extension methods
3. THE Core.CodeGen SHALL generate arity-based unit tests for all generated OneOf types and their methods
4. THE Core.CodeGen SHALL generate unit tests that maintain the same test patterns as existing manually written tests
5. THE Core.CodeGen SHALL generate unit tests using the same Core.XUnit assertion patterns found in existing tests

### Requirement 5

**User Story:** As a developer extending the Core.CodeGen system, I want an enhanced Design framework, so that I can easily create new generators with complex method signatures and implementations.

#### Acceptance Criteria

1. WHEN creating new generators, THE Design System SHALL support generic method constraints and where clauses
2. THE Design System SHALL support async method generation with proper Task and ValueTask return types
3. THE Design System SHALL support extension method generation with proper this parameter handling
4. THE Design System SHALL support method overloading with different parameter combinations
5. THE Design System SHALL generate XML documentation for all public generated methods except unit tests

### Requirement 6

**User Story:** As a developer running the code generation, I want generators created for all existing manually written extension categories, so that the entire Core library can be generated consistently.

#### Acceptance Criteria

1. WHEN the code generation runs, THE Core.CodeGen SHALL create generators for all existing error handling extension methods
2. THE Core.CodeGen SHALL create generators for all existing async extension methods in Tasks and ValueTasks directories
3. THE Core.CodeGen SHALL create generators for ResultMatchExtensions based on existing manual implementations
4. THE Core.CodeGen SHALL create generators for Option extension methods based on existing manual implementations

### Requirement 7

**User Story:** As a developer maintaining the codebase, I want the code generation system to be configurable and extensible, so that I can easily add new types and modify generation behavior without changing core logic.

#### Acceptance Criteria

1. WHEN adding new generator types, THE Core.CodeGen SHALL support registration through the CodeGeneratorFactory
2. THE Core.CodeGen SHALL support different file organization modes (separate files vs single file with regions)
3. THE Core.CodeGen SHALL support configurable target arities for different type categories
4. THE Core.CodeGen SHALL support namespace customization for different generated types
5. THE Core.CodeGen SHALL validate configuration parameters and provide clear error messages for invalid inputs

### Requirement 8

**User Story:** As a developer using the generated code, I want consistent API patterns across all generated methods, so that I can predict method behavior and signatures across different arities and types.

#### Acceptance Criteria

1. WHEN using generated extension methods, THE Core.CodeGen SHALL ensure consistent parameter naming across all arities
2. THE Core.CodeGen SHALL ensure consistent return type patterns for similar operations
3. THE Core.CodeGen SHALL ensure consistent error handling behavior across all generated methods
4. THE Core.CodeGen SHALL generate XML documentation for all public methods except unit tests
5. THE Core.CodeGen SHALL ensure consistent async method naming conventions (e.g., BindAsync, MapAsync)