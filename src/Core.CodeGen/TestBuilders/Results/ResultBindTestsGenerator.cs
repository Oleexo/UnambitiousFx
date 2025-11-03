using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Bind extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultBindTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultBindTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultBindTestsGenerator(string               baseNamespace,
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
        var cw = new ClassWriter($"ResultBindSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Bind" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Bind" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Bind" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Bind_Arity{arity}_Success_ShouldTransform",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Bind_Arity{arity}_Failure_ShouldNotTransform",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        return new MethodWriter($"BindTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        return new MethodWriter($"BindTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        return new MethodWriter($"BindValueTask_Arity{arity}_Success_ShouldTransform",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        return new MethodWriter($"BindValueTask_Arity{arity}_Failure_ShouldNotTransform",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateBindSyncCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateBindSyncCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateBindTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateBindTaskCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateBindValueTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return $"""
                {testValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateBindValueTaskCall(arity);
        return $"""
                {creation}
                {call}
                Assert.False(transformedResult.IsSuccess);
                """;
    }

    private string GenerateBindSyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Bind(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = result.Bind(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await taskResult.BindAsync(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await taskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindValueTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await valueTaskResult.BindAsync(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await valueTaskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return """
                   Assert.True(transformedResult.IsSuccess);
                   Assert.True(transformedResult.TryGet(out var boundValue));
                   Assert.Equal(84, boundValue);
                   """;
        }

        return "Assert.True(transformedResult.IsSuccess);";
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
            "UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks"
        ];
    }
}
