using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Ensure validation extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultEnsureTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultEnsureTests";
    private const string ExtensionsNamespace = "Results.Extensions.Validations";

    public ResultEnsureTestsGenerator(string               baseNamespace,
                                      FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        var classes = new List<ClassWriter>();
        var sync    = GenerateSyncTests(arity);
        if (sync != null) {
            classes.Add(sync);
        }

        var task = GenerateTaskTests(arity);
        if (task != null) {
            task.UnderClass = ClassName;
            classes.Add(task);
        }

        var valueTask = GenerateValueTaskTests(arity);
        if (valueTask != null) {
            valueTask.UnderClass = ClassName;
            classes.Add(valueTask);
        }

        return classes;
    }

    private ClassWriter? GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Ensure" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncValidationFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Ensure" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskValidationFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Ensure" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskValidationFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    #region Sync Methods

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Ensure_Arity{arity}_ValidCondition_ShouldSucceed",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Ensure_Arity{arity}_FailureResult_ShouldNotValidate",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncValidationFailureTest(ushort arity) {
        return new MethodWriter($"Ensure_Arity{arity}_InvalidCondition_ShouldFail",
                                "void",
                                GenerateSyncValidationFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion

    #region Task Methods

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"EnsureTask_Arity{arity}_ValidCondition_ShouldSucceed",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"EnsureTask_Arity{arity}_FailureResult_ShouldNotValidate",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskValidationFailureTest(ushort arity) {
        return new MethodWriter($"EnsureTask_Arity{arity}_InvalidCondition_ShouldFail",
                                "async Task",
                                GenerateTaskValidationFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion

    #region ValueTask Methods

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"EnsureValueTask_Arity{arity}_ValidCondition_ShouldSucceed",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"EnsureValueTask_Arity{arity}_FailureResult_ShouldNotValidate",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskValidationFailureTest(ushort arity) {
        return new MethodWriter($"EnsureValueTask_Arity{arity}_InvalidCondition_ShouldFail",
                                "async Task",
                                GenerateValueTaskValidationFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion

    #region Sync Body Generation

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity);
        var errorFactory = GenerateErrorFactory(arity);
        var call         = GenerateEnsureSyncCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation     = GenerateFailureResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity);
        var errorFactory = GenerateErrorFactory(arity);
        var call         = GenerateEnsureSyncCall(arity);
        return $"""
                // Given
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                Assert.False(ensuredResult.IsSuccess);
                """;
    }

    private string GenerateSyncValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateResultCreation(arity);
        var predicate    = GenerateInvalidPredicate(arity);
        var errorFactory = GenerateErrorFactory(arity);
        var call         = GenerateEnsureSyncCall(arity);
        var assertions   = GenerateValidationFailureAssertions();
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    #endregion

    #region Task Body Generation

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateTaskResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation     = GenerateTaskFailureResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        return $"""
                // Given
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                Assert.False(ensuredResult.IsSuccess);
                """;
    }

    private string GenerateTaskValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateTaskResultCreation(arity);
        var predicate    = GenerateInvalidPredicate(arity, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        var assertions   = GenerateValidationFailureAssertions();
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    #endregion

    #region ValueTask Body Generation

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateValueTaskResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation     = GenerateValueTaskFailureResultCreation(arity);
        var predicate    = GenerateValidPredicate(arity, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                Assert.False(ensuredResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateValueTaskResultCreation(arity);
        var predicate    = GenerateInvalidPredicate(arity, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        var assertions   = GenerateValidationFailureAssertions();
        return $"""
                // Given
                {testValues}
                {creation}
                {predicate}
                {errorFactory}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    #endregion

    #region Call Generation

    private string GenerateEnsureSyncCall(ushort arity) {
        return "var ensuredResult = result.Ensure(predicate, errorFactory);";
    }

    private string GenerateEnsureTaskCall(ushort arity) {
        return "var ensuredResult = await taskResult.EnsureAsync(predicate, errorFactory);";
    }

    private string GenerateEnsureValueTaskCall(ushort arity) {
        return "var ensuredResult = await valueTaskResult.EnsureAsync(predicate, errorFactory);";
    }

    #endregion

    #region Predicate and Factory Generation

    private string GenerateValidPredicate(ushort  arity,
                                          string? asyncType = null) {
        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));

        if (asyncType != null) {
            // Async predicate: Func<T1, ..., TN, Task<bool>> or Func<T1, ..., TN, ValueTask<bool>>
            var condition = arity == 1
                                ? "x1 > 0"
                                : GenerateValidCondition(arity);
            return $"Func<{typeParams}, {asyncType}<bool>> predicate = ({parameters}) => {asyncType}.FromResult({condition});";
        }
        else {
            // Sync predicate: Func<T1, ..., TN, bool>
            var condition = GenerateValidCondition(arity);
            return $"Func<{typeParams}, bool> predicate = ({parameters}) => {condition};";
        }
    }

    private string GenerateInvalidPredicate(ushort  arity,
                                            string? asyncType = null) {
        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));

        if (asyncType != null) {
            // Async predicate: Func<T1, ..., TN, Task<bool>> or Func<T1, ..., TN, ValueTask<bool>>
            var condition = arity == 1
                                ? "x1 < 0"
                                : GenerateInvalidCondition(arity);
            return $"Func<{typeParams}, {asyncType}<bool>> predicate = ({parameters}) => {asyncType}.FromResult({condition});";
        }
        else {
            // Sync predicate: Func<T1, ..., TN, bool>
            var condition = GenerateInvalidCondition(arity);
            return $"Func<{typeParams}, bool> predicate = ({parameters}) => {condition};";
        }
    }

    private string GenerateErrorFactory(ushort  arity,
                                        string? asyncType = null) {
        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var errorMessage = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(i => $"{{x{i}}}"));

        if (asyncType != null) {
            // Async factory: Func<T1, ..., TN, Task<Exception>> or Func<T1, ..., TN, ValueTask<Exception>>
            return
                $"Func<{typeParams}, {asyncType}<Exception>> errorFactory = ({parameters}) => {asyncType}.FromResult<Exception>(new InvalidOperationException($\"Validation failed: {errorMessage}\"));";
        }

        // Sync factory: Func<T1, ..., TN, Exception>
        return $"Func<{typeParams}, Exception> errorFactory = ({parameters}) => new InvalidOperationException($\"Validation failed: {errorMessage}\");";
    }

    private string GenerateValidCondition(ushort arity) {
        if (arity == 1) {
            return "true";
        }

        var conditions = new List<string>();
        for (var i = 1; i <= arity; i++) {
            conditions.Add(i switch {
                1 => "x1 > 0",
                2 => "!string.IsNullOrEmpty(x2)",
                3 => "x3 == true",
                4 => "x4 > 0",
                5 => "x5 > 0",
                _ => $"!string.IsNullOrEmpty(x{i})"
            });
        }

        return string.Join(" && ", conditions);
    }

    private string GenerateInvalidCondition(ushort arity) {
        if (arity == 1) {
            return "false";
        }

        var conditions = new List<string>();
        for (var i = 1; i <= arity; i++) {
            conditions.Add(i switch {
                1 => "x1 < 0",
                2 => "string.IsNullOrEmpty(x2)",
                3 => "x3 == false",
                4 => "x4 < 0",
                5 => "x5 < 0",
                _ => $"string.IsNullOrEmpty(x{i})"
            });
        }

        return string.Join(" || ", conditions);
    }

    #endregion

    #region Assertions

    private string GenerateEnsureSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.True(ensuredResult.IsSuccess);
                   Assert.True(ensuredResult.TryGet(out var value));
                   Assert.Equal(42, value);
                   """;
        }

        return "Assert.True(ensuredResult.IsSuccess);";
    }

    private string GenerateValidationFailureAssertions() {
        return """
               Assert.False(ensuredResult.IsSuccess);
               var errorsList = ensuredResult.Errors.ToList();
               Assert.NotEmpty(errorsList);
               var exceptionalError = errorsList.OfType<UnambitiousFx.Core.Results.Reasons.ExceptionalError>().FirstOrDefault();
               Assert.NotNull(exceptionalError);
               Assert.IsType<InvalidOperationException>(exceptionalError.Exception);
               Assert.Contains("Validation failed", exceptionalError.Exception.Message);
               """;
    }

    #endregion

    #region Helper Methods

    private string GenerateResultCreation(ushort arity) {
        if (arity == 1) {
            return "var result = Result.Success(value1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateTaskResultCreation(ushort arity) {
        if (arity == 1) {
            return "var taskResult = Task.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var taskResult = Task.FromResult(Result.Success({values}));";
    }

    private string GenerateValueTaskResultCreation(ushort arity) {
        if (arity == 1) {
            return "var valueTaskResult = ValueTask.FromResult(Result.Success(value1));";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Success({values}));";
    }

    private string GenerateFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }

    private string GenerateTaskFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureResultCreation(ushort arity) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    private string GetTestValue(int index) {
        return index switch {
            1 => "42",
            2 => "\"test\"",
            3 => "true",
            4 => "3.14",
            5 => "123L",
            _ => $"\"value{index}\""
        };
    }

    private string GetTestType(int index) {
        return index switch {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "long",
            _ => "string"
        };
    }

    private IEnumerable<string> GetUsings() {
        return [
            "System",
            "System.Linq",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Extensions.Validations",
            "UnambitiousFx.Core.Results.Extensions.Validations.Tasks",
            "UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks"
        ];
    }

    #endregion
}
