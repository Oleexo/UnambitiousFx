using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Match async methods (MatchAsync for Task and ValueTask results).
///     Generates success and failure tests for all arities.
/// </summary>
internal sealed class ResultMatchTestsGenerator : BaseCodeGenerator {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultMatchTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultMatchTestsGenerator(string               baseNamespace,
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

        var taskClass = GenerateTaskTests(arity);
        if (taskClass != null) {
            taskClass.UnderClass = ClassName;
            classes.Add(taskClass);
        }

        var valueTaskClass = GenerateValueTaskTests(arity);
        if (valueTaskClass != null) {
            valueTaskClass.UnderClass = ClassName;
            classes.Add(valueTaskClass);
        }

        return classes;
    }

    private ClassWriter? GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchTaskTestsArity{arity}", Visibility.Public) {
            Region = $"Arity {arity} - Task Tests"
        };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultMatchValueTaskTestsArity{arity}", Visibility.Public) {
            Region = $"Arity {arity} - ValueTask Tests"
        };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity) {
        var body = GenerateTaskSuccessTestBody(arity);
        return new MethodWriter($"MatchTask_Arity{arity}_Success_ShouldReturnSuccessValue",
                                "async Task",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity) {
        var body = GenerateTaskFailureTestBody(arity);
        return new MethodWriter($"MatchTask_Arity{arity}_Failure_ShouldReturnFailureValue",
                                "async Task",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) {
        var body = GenerateValueTaskSuccessTestBody(arity);
        return new MethodWriter($"MatchValueTask_Arity{arity}_Success_ShouldReturnSuccessValue",
                                "async Task",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) {
        var body = GenerateValueTaskFailureTestBody(arity);
        return new MethodWriter($"MatchValueTask_Arity{arity}_Failure_ShouldReturnFailureValue",
                                "async Task",
                                body,
                                attributes: [new FactAttributeReference()],
                                usings: GetRequiredUsings());
    }

    private string GenerateTaskSuccessTestBody(ushort arity) {
        var          testValues     = GenerateTestValues(arity);
        var          resultCreation = GenerateTaskResultCreation(arity);
        var          methodCall     = GenerateTaskMatchCall(arity, "success");
        const string assertions     = "Assert.Equal(\"success\", matchResult);";
        return $"""
                // Given: A successful Task<Result>
                {testValues}
                {resultCreation}

                // When: Calling MatchAsync
                {methodCall}

                // Then: Should return success value
                {assertions}
                """;
    }

    private string GenerateTaskFailureTestBody(ushort arity) {
        var          resultCreation = GenerateTaskFailureResultCreation(arity);
        var          methodCall     = GenerateTaskMatchCall(arity, "success");
        const string assertions     = "Assert.Equal(\"failure\", matchResult);";
        return $"""
                // Given: A failed Task<Result>
                {resultCreation}

                // When: Calling MatchAsync
                {methodCall}

                // Then: Should return failure value
                {assertions}
                """;
    }

    private string GenerateValueTaskSuccessTestBody(ushort arity) {
        var          testValues     = GenerateTestValues(arity);
        var          resultCreation = GenerateValueTaskResultCreation(arity);
        var          methodCall     = GenerateValueTaskMatchCall(arity, "success");
        const string assertions     = "Assert.Equal(\"success\", matchResult);";
        return $"""
                // Given: A successful ValueTask<Result>
                {testValues}
                {resultCreation}

                // When: Calling MatchAsync
                {methodCall}

                // Then: Should return success value
                {assertions}
                """;
    }

    private string GenerateValueTaskFailureTestBody(ushort arity) {
        var          resultCreation = GenerateValueTaskFailureResultCreation(arity);
        var          methodCall     = GenerateValueTaskMatchCall(arity, "success");
        const string assertions     = "Assert.Equal(\"failure\", matchResult);";
        return $"""
                // Given: A failed ValueTask<Result>
                {resultCreation}

                // When: Calling MatchAsync
                {methodCall}

                // Then: Should return failure value
                {assertions}
                """;
    }

    private string GenerateTaskMatchCall(ushort arity,
                                         string successValue) {
        var successLambda = GenerateSuccessLambda(arity, false, successValue);
        return $"var matchResult = await taskResult.MatchAsync({successLambda}, errors => Task.FromResult(\"failure\"));";
    }

    private string GenerateValueTaskMatchCall(ushort arity,
                                              string successValue) {
        var successLambda = GenerateSuccessLambda(arity, true, successValue);
        return $"var matchResult = await valueTaskResult.MatchAsync({successLambda}, errors => ValueTask.FromResult(\"failure\"));";
    }

    private string GenerateSuccessLambda(ushort arity,
                                         bool   isValueTask,
                                         string successValue) {
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";
        if (arity == 1) {
            return $"x => {asyncType}.FromResult(\"{successValue}\")";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        return $"({parameters}) => {asyncType}.FromResult(\"{successValue}\")";
    }

    private string GenerateTestValues(ushort arity) {
        return string.Join("\n", Enumerable.Range(1, arity)
                                           .Select(i => $"var value{i} = {GetTestValue(i)};"));
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

    private IEnumerable<string> GetRequiredUsings() {
        return [
            "System",
            "System.Threading.Tasks",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Reasons",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks",
            "UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks"
        ];
    }
}
