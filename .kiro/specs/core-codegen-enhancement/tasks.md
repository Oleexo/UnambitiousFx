# Implementation Plan

- [x] 1. Refactor Design Framework to follow C# best practices
  - Refactor MethodWriter to be immutable with builder pattern for construction
  - Add support for generic method constraints with complex where clauses
  - Add extension method generation with proper `this` parameter handling
  - Integrate XML documentation generation into the writer
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_

- [x] 1.1 Refactor MethodWriter class to immutable design
  - Convert MethodWriter to immutable class with readonly fields
  - Create MethodWriterBuilder for fluent construction
  - Add support for extension methods with `this` parameter
  - Add support for generic constraints and where clauses
  - _Requirements: 5.1, 5.3, 5.4_

- [x] 1.2 Enhance DocumentationWriter with builder pattern
  - Refactor DocumentationWriter to follow builder pattern
  - Add support for type parameter documentation
  - Add support for exception documentation
  - Ensure proper XML escaping and formatting
  - _Requirements: 5.5_

- [x] 1.3 Create GenericConstraint support classes
  - Create GenericConstraint record for constraint representation
  - Add GenericConstraintType enum for different constraint types
  - Update GenericParameter to support multiple constraints
  - _Requirements: 5.1_

- [x] 2. Complete Error Handling Extension Generators
  - Create remaining error handling generators for manual extension methods
  - Register new generators in CodeGeneratorFactory
  - Remove manual code files after successful generation and validation
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

- [x] 2.1 Create ResultMapErrorExtensionsCodeGenerator
  - Analyze existing MapError extension methods in ResultExtensions.MapError.cs
  - Generate arity-based versions for Result<T1>, Result<T1,T2>, etc.
  - Support both basic and policy-based overloads
  - Generate proper XML documentation for all methods
  - _Requirements: 1.1_

- [x] 2.2 Create ResultHasErrorExtensionsCodeGenerator
  - Analyze existing HasError extension methods in ResultExtensions.HasError.cs
  - Generate arity-based versions with proper generic type parameter handling
  - Support both error type and exception type checking
  - Generate proper XML documentation for all methods
  - _Requirements: 1.1_

- [x] 2.3 Create ResultFilterErrorExtensionsCodeGenerator
  - Analyze existing FilterError extension methods in ResultExtensions.FilterError.cs
  - Generate arity-based versions for all Result arities
  - Generate proper XML documentation for all methods
  - _Requirements: 1.1_

- [x] 2.4 Create ResultHasExceptionExtensionsCodeGenerator
  - Analyze existing HasException extension methods in ResultExtensions.HasException.cs
  - Generate arity-based versions for exception type checking
  - Generate proper XML documentation for all methods
  - _Requirements: 1.1_

- [x] 2.5 Create ResultRecoveryExtensionsCodeGenerator
  - Analyze existing Recovery extension methods in ResultExtensions.Recovery.cs
  - Generate arity-based versions for error recovery scenarios
  - Generate proper XML documentation for all methods
  - _Requirements: 1.1_

- [x] 2.6 Create ResultAppendErrorExtensionsCodeGenerator
  - Analyze existing AppendError extension methods in ResultExtensions.AppendError.cs
  - Generate arity-based versions for appending errors to existing results
  - Generate proper XML documentation for all methods
  - _Requirements: 1.2_

- [x] 2.7 Create ResultPrependErrorExtensionsCodeGenerator
  - Analyze existing PrependError extension methods in ResultExtensions.PrependError.cs
  - Generate arity-based versions for prepending errors to existing results
  - Generate proper XML documentation for all methods
  - _Requirements: 1.2_

- [x] 2.8 Create ResultAccumulateExtensionsCodeGenerator
  - Analyze existing Accumulate extension methods in ResultExtensions.Accumulate.cs
  - Generate arity-based versions for accumulating errors across operations
  - Generate proper XML documentation for all methods
  - _Requirements: 1.3_

- [x] 2.9 Create ResultFindErrorExtensionsCodeGenerator
  - Analyze existing FindError extension methods in ResultExtensions.FindError.cs
  - Generate arity-based versions for finding specific errors in results
  - Generate proper XML documentation for all methods
  - _Requirements: 1.4_

- [x] 2.10 Create ResultMatchErrorExtensionsCodeGenerator
  - Analyze existing MatchError extension methods in ResultExtensions.MatchError.cs
  - Generate arity-based versions for pattern matching on error types
  - Generate proper XML documentation for all methods
  - _Requirements: 1.4_

- [x] 2.11 Create ResultShapeErrorExtensionsCodeGenerator
  - Analyze existing ShapeError extension methods in ResultExtensions.ShapeError.cs
  - Generate arity-based versions for transforming error structure
  - Generate proper XML documentation for all methods
  - _Requirements: 1.5_

  - [x] 2.12 Create ResultMapErrorsExtensionsCodeGenerator
  - Analyze existing ShapeError extension methods in ResultExtensions.MapErrors.cs
  - Generate arity-based versions for transforming error structure
  - Generate proper XML documentation for all methods
  - _Requirements: 1.5_

- [ ] 3. Create Async Extension Generators
  - Analyze existing async extension methods in Tasks and ValueTasks subdirectories
  - Create generators for async versions of all extension categories
  - Support both Task and ValueTask variants with proper naming conventions
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_

- [x] 3.1 Create async error handling generators
  - Create async versions of all error handling extension generators
  - Support both Task and ValueTask variants for MapError, MapErrors, HasError, FilterError, Recovery, AppendError, PrependError, FindError, MatchError, ShapeError.
  - Generate proper async method naming conventions (e.g., MapErrorAsync)
  - Generate proper XML documentation for all async methods
  - _Requirements: 3.1, 3.4, 3.5_

- [x] 3.2 Create async transformation and side-effect generators
  - Create async versions for transformation methods (Bind, Map, Then, Try, Zip, Flatten)
  - Generate Task and ValueTask versions with proper naming conventions
  - Generate proper XML documentation for all async methods
  - _Requirements: 3.2, 3.3, 3.4, 3.5_

- [ ] 4. Enhance Test Generation System
  - Enhance existing test generation to cover all new extension categories
  - Generate arity-based unit tests for all generated extension methods
  - Ensure tests follow existing patterns and use Core.XUnit assertion patterns
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5_

- [x] 4.1 Enhance existing test generators for new extension categories
  - Update ResultTestGenerator to handle error handling extensions
  - Update ResultTestGenerator to handle async extensions
  - Generate tests that follow existing patterns in test/Core.Tests/Results/Extensions
  - _Requirements: 4.1, 4.2, 4.5_

- [ ] 4.2 Create test generators for new error handling methods
  - Generate tests for AppendError, PrependError, Accumulate methods
  - Generate tests for FindError, MatchError, ShapeError methods
  - Use Core.XUnit assertion patterns from existing tests
  - Generate tests for both success and failure scenarios
  - _Requirements: 4.1, 4.2, 4.5_

- [ ] 4.3 Create test generators for async methods
  - Generate async test methods with proper async/await patterns
  - Use Core.XUnit assertion patterns from existing tests
  - Generate tests for both Task and ValueTask variants
  - Generate tests for all async extension categories
  - _Requirements: 4.1, 4.2, 4.5_

- [ ] 5. Update CodeGeneratorFactory registration
  - Register all new generators in CodeGeneratorFactory
  - Ensure all generator categories are properly orchestrated
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5_

- [ ] 5.1 Register remaining error handling generators
  - Add ResultAppendErrorExtensionsCodeGenerator to factory
  - Add ResultPrependErrorExtensionsCodeGenerator to factory
  - Add ResultAccumulateExtensionsCodeGenerator to factory
  - Add ResultFindErrorExtensionsCodeGenerator to factory
  - Add ResultMatchErrorExtensionsCodeGenerator to factory
  - Add ResultShapeErrorExtensionsCodeGenerator to factory
  - _Requirements: 7.1_

- [ ] 5.2 Register async generators
  - Add async error handling generators to factory
  - Add async transformation and side-effect generators to factory
  - _Requirements: 7.1_

- [ ] 5.3 Register additional generators
  - Add ResultMatchExtensionsCodeGenerator to factory
  - Add OptionExtensionsCodeGenerator to factory
  - _Requirements: 7.1_

- [ ] 6. Validate and finalize implementation
  - Ensure all generated code compiles and tests pass
  - Validate API consistency across all generated methods
  - Remove manual code files after successful generation
  - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5_

- [ ] 6.1 Validate generated code quality
  - Ensure consistent parameter naming across all generated methods
  - Ensure consistent return type patterns for similar operations
  - Ensure consistent error handling behavior across all generated methods
  - Ensure consistent async method naming conventions
  - _Requirements: 8.1, 8.2, 8.4, 8.5_

- [ ] 6.2 Run integration tests
  - Verify all generated code compiles successfully
  - Verify all generated tests pass
  - Verify behavioral equivalence with manual code
  - Run performance tests for large arity ranges
  - _Requirements: 8.1, 8.2, 8.3_

- [ ] 6.3 Clean up manual code files
  - Remove manual error handling extension files after validation
  - Keep backup files for reference during transition period
  - Update documentation to reflect generated code usage
  - _Requirements: 8.3, 8.4_