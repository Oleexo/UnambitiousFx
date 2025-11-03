using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ValueOrThrow extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultValueOrThrowTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultValueOrThrowTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrThrowTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultValueOrThrowSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOrThrow" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateSyncFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrThrowTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ValueOrThrow" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateTaskFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultValueOrThrowValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ValueOrThrow" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateValueTaskFailureWithFactoryTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOrThrow_Arity{arity}_Success_ShouldReturnValue",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"ValueOrThrow_Arity{arity}_Failure_ShouldThrowException",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "void",
                                GenerateSyncSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowWithFactory_Arity{arity}_Failure_ShouldThrowCustomException",
                                "void",
                                GenerateSyncFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowTask_Arity{arity}_Failure_ShouldThrowException",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowTaskWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateTaskSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowTaskWithFactory_Arity{arity}_Failure_ShouldThrowCustomException",
                                "async Task",
                                GenerateTaskFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowValueTask_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowValueTask_Arity{arity}_Failure_ShouldThrowException",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowValueTaskWithFactory_Arity{arity}_Success_ShouldReturnValue",
                                "async Task",
                                GenerateValueTaskSuccessWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureWithFactoryTest(ushort arity) {
        return new MethodWriter($"ValueOrThrowValueTaskWithFactory_Arity{arity}_Failure_ShouldThrowCustomException",
                                "async Task",
                                GenerateValueTaskFailureWithFactoryBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateValueOrThrowSyncCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = GenerateValueOrThrowSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When & Then
                Assert.Throws<Exception>(() => {call.Replace("var actualValue = ", "").TrimEnd(';')});
                """;
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowSyncWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity) {
        var failureCreation   = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowSyncWithFactoryCall(arity);
        return $"""
                // Given
                {failureCreation}
                {factoryDefinition}
                // When & Then
                Assert.Throws<InvalidOperationException>(() => {call.Replace("var actualValue = ", "").TrimEnd(';')});
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateValueOrThrowTaskCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateValueOrThrowTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When & Then
                await Assert.ThrowsAsync<Exception>(async () => {call.Replace("var actualValue = await ", "await ").TrimEnd(';')});
                """;
    }

    private string GenerateTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateTaskResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowTaskWithFactoryCall(arity);
        return $"""
                // Given
                {creation}
                {factoryDefinition}
                // When & Then
                await Assert.ThrowsAsync<InvalidOperationException>(async () => {call.Replace("var actualValue = await ", "await ").TrimEnd(';')});
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateValueOrThrowValueTaskCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateValueOrThrowValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When & Then
                await Assert.ThrowsAsync<Exception>(async () => {call.Replace("var actualValue = await ", "await ").TrimEnd(';')});
                """;
    }

    private string GenerateValueTaskSuccessWithFactoryBody(ushort arity) {
        var testValues        = GenerateTestValues(arity);
        var creation          = GenerateValueTaskResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowValueTaskWithFactoryCall(arity);
        var assertions        = GenerateValueOrThrowSuccessAssertions(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                {factoryDefinition}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureWithFactoryBody(ushort arity) {
        var creation          = GenerateValueTaskFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call              = GenerateValueOrThrowValueTaskWithFactoryCall(arity);
        return $"""
                // Given
                {creation}
                {factoryDefinition}
                // When & Then
                await Assert.ThrowsAsync<InvalidOperationException>(async () => {call.Replace("var actualValue = await ", "await ").TrimEnd(';')});
                """;
    }

    private string GenerateValueOrThrowSyncCall(ushort arity) {
        return "var actualValue = result.ValueOrThrow();";
    }

    private string GenerateValueOrThrowSyncWithFactoryCall(ushort arity) {
        return "var actualValue = result.ValueOrThrow(exceptionFactory);";
    }

    private string GenerateValueOrThrowTaskCall(ushort arity) {
        return "var actualValue = await taskResult.ValueOrThrowAsync();";
    }

    private string GenerateValueOrThrowTaskWithFactoryCall(ushort arity) {
        return "var actualValue = await taskResult.ValueOrThrowAsync(exceptionFactory);";
    }

    private string GenerateValueOrThrowValueTaskCall(ushort arity) {
        return "var actualValue = await valueTaskResult.ValueOrThrowAsync();";
    }

    private string GenerateValueOrThrowValueTaskWithFactoryCall(ushort arity) {
        return "var actualValue = await valueTaskResult.ValueOrThrowAsync(exceptionFactory);";
    }

    private string GenerateValueOrThrowSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.Equal(value1, actualValue);
                   """;
        }

        var assertions = string.Join("\n", Enumerable.Range(1, arity)
                                                     .Select(i => $"Assert.Equal(value{i}, actualValue.Item{i});"));
        return assertions;
    }

    private string GenerateExceptionFactoryDefinition() {
        return "Func<IEnumerable<IError>, Exception> exceptionFactory = errors => new InvalidOperationException(\"Custom error\");";
    }

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
        return index switch { 1 => "42", 2 => "\"test\"", 3 => "true", 4 => "3.14", 5 => "123L", _ => $"\"value{index}\"" };
    }

    private string GetTestType(int index) {
        return index switch { 1 => "int", 2 => "string", 3 => "bool", 4 => "double", 5 => "long", _ => "string" };
    }

    private IEnumerable<string> GetUsings() {
        return [
            "System",
            "System.Collections.Generic",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Reasons",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks"
        ];
    }
}
