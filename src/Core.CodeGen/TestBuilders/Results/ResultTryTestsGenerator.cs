using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Try extension methods (sync, Task, ValueTask) including exception path.
/// </summary>
internal sealed class ResultTryTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultTryTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultTryTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultTrySyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Try" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncExceptionTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultTryTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Try" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskExceptionTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultTryValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Try" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskExceptionTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateTrySyncCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateTrySyncCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateSyncExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateTrySyncExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateTryTaskCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateTryTaskCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateTaskExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateTryTaskExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateTryValueTaskCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateTryValueTaskCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateTryValueTaskExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateTrySyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Try<int, string>(x => x.ToString() + \"_tried\");";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_tried\""));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => ({tupleItems}));";
    }

    private string GenerateTryTaskCall(ushort arity) {
        if (arity == 1) {
            return "var result = await taskResult;\nvar transformedResult = result.Try<int, string>(x => x.ToString() + \"_tried\");";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_tried\""));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"var result = await taskResult;\nvar transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => ({tupleItems}));";
    }

    private string GenerateTryValueTaskCall(ushort arity) {
        if (arity == 1) {
            return "var result = await valueTaskResult;\nvar transformedResult = result.Try<int, string>(x => x.ToString() + \"_tried\");";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_tried\""));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"var result = await valueTaskResult;\nvar transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => ({tupleItems}));";
    }

    private string GenerateTrySyncExceptionCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Try<int, string>(x => throw new InvalidOperationException(\"Test exception\"));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => throw new InvalidOperationException(\"Test exception\"));";
    }

    private string GenerateTryTaskExceptionCall(ushort arity) {
        if (arity == 1) {
            return "var result = await taskResult;\nvar transformedResult = result.Try<int, string>(x => throw new InvalidOperationException(\"Test exception\"));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return
            $"var result = await taskResult;\nvar transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => throw new InvalidOperationException(\"Test exception\"));";
    }

    private string GenerateTryValueTaskExceptionCall(ushort arity) {
        if (arity == 1) {
            return "var result = await valueTaskResult;\nvar transformedResult = result.Try<int, string>(x => throw new InvalidOperationException(\"Test exception\"));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var inputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return
            $"var result = await valueTaskResult;\nvar transformedResult = result.Try<{inputTypes}, {outputTypes}>(({parameters}) => throw new InvalidOperationException(\"Test exception\"));";
    }

    private string GenerateTrySuccessAssertions(ushort arity) {
        return arity == 1
                   ? """
                     Assert.True(transformedResult.IsSuccess);
                     Assert.True(transformedResult.TryGet(out var triedValue));
                     Assert.Equal("42_tried", triedValue);
                     """
                   : "Assert.True(transformedResult.IsSuccess);";
    }

    private string GenerateTryExceptionAssertions(ushort arity) {
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"""
                Assert.False(transformedResult.IsSuccess);
                Assert.True(transformedResult.HasException<InvalidOperationException, {outputTypes}>());
                """;
    }

    private string GenerateResultCreation(ushort arity) {
        return arity == 1
                   ? "var result = Result.Success(value1);"
                   : $"var result = Result.Success({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"))});";
    }

    private string GenerateTaskResultCreation(ushort arity) {
        return arity == 1
                   ? "var taskResult = Task.FromResult(Result.Success(value1));"
                   : $"var taskResult = Task.FromResult(Result.Success({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"))}));";
    }

    private string GenerateValueTaskResultCreation(ushort arity) {
        return arity == 1
                   ? "var valueTaskResult = ValueTask.FromResult(Result.Success(value1));"
                   : $"var valueTaskResult = ValueTask.FromResult(Result.Success({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"value{i}"))}));";
    }

    private string GenerateFailureResultCreation(ushort arity) {
        var tp = string.Join(", ", Enumerable.Range(1, arity)
                                             .Select(GetTestType));
        return $"var result = Result.Failure<{tp}>(\"Test error\");";
    }

    private string GenerateTaskFailureResultCreation(ushort arity) {
        var tp = string.Join(", ", Enumerable.Range(1, arity)
                                             .Select(GetTestType));
        return $"var taskResult = Task.FromResult(Result.Failure<{tp}>(\"Test error\"));";
    }

    private string GenerateValueTaskFailureResultCreation(ushort arity) {
        var tp = string.Join(", ", Enumerable.Range(1, arity)
                                             .Select(GetTestType));
        return $"var valueTaskResult = ValueTask.FromResult(Result.Failure<{tp}>(\"Test error\"));";
    }

    private string GenerateTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
    }

    private string GetTestValue(int index) {
        return index switch { 1 => "42", 2 => "\"test\"", 3 => "true", 4 => "3.14", 5 => "123L", _ => $"\"value{index}\"" };
    }

    private string GetTestType(int index) {
        return index switch { 1 => "int", 2 => "string", 3 => "bool", 4 => "double", 5 => "long", _ => "string" };
    }

    private IEnumerable<string> GetUsings() {
        return [
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Extensions.Transformations",
            "UnambitiousFx.Core.Results.Extensions.Transformations.Tasks",
            "UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks",
            "UnambitiousFx.Core.Results.Extensions.ErrorHandling"
        ];
    }

    #region Sync Methods

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Try_Arity{arity}_Success_ShouldTransform",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Try_Arity{arity}_Failure_ShouldNotTransform",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncExceptionTest(ushort arity) {
        return new MethodWriter($"Try_Arity{arity}_Exception_ShouldCaptureException",
                                "void",
                                GenerateSyncExceptionBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion

    #region Task Methods

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"TryTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"TryTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskExceptionTest(ushort arity) {
        return new MethodWriter($"TryTask_Arity{arity}_Exception_ShouldCaptureException",
                                "async Task",
                                GenerateTaskExceptionBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion

    #region ValueTask Methods

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"TryValueTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"TryValueTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskExceptionTest(ushort arity) {
        return new MethodWriter($"TryValueTask_Arity{arity}_Exception_ShouldCaptureException",
                                "async Task",
                                GenerateValueTaskExceptionBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    #endregion
}
