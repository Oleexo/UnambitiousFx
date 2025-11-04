using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Try extension methods (sync, Task, ValueTask) including exception path.
/// </summary>
internal sealed class ResultTryTestsGenerator : ResultTestGeneratorBase {
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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultTrySyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Try" };
        cw.AddMethod(new MethodWriter($"Try_Arity{arity}_Success_ShouldTransform",
                                      "void",
                                      GenerateSyncSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Try_Arity{arity}_Failure_ShouldNotTransform",
                                      "void",
                                      GenerateSyncFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Try_Arity{arity}_Exception_ShouldReturnExceptionalError",
                                      "void",
                                      GenerateSyncExceptionBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultTryTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Try" };
        cw.AddMethod(new MethodWriter($"TryTask_Arity{arity}_Success_ShouldTransform",
                                      "async Task",
                                      GenerateTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TryTask_Arity{arity}_Failure_ShouldNotTransform",
                                      "async Task",
                                      GenerateTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TryTask_Arity{arity}_Exception_ShouldReturnExceptionalError",
                                      "async Task",
                                      GenerateTaskExceptionBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultTryValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Try" };
        cw.AddMethod(new MethodWriter($"TryValueTask_Arity{arity}_Success_ShouldTransform",
                                      "async Task",
                                      GenerateValueTaskSuccessBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TryValueTask_Arity{arity}_Failure_ShouldNotTransform",
                                      "async Task",
                                      GenerateValueTaskFailureBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"TryValueTask_Arity{arity}_Exception_ShouldReturnExceptionalError",
                                      "async Task",
                                      GenerateValueTaskExceptionBody(arity),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateTrySyncCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateTrySyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateSyncExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateTrySyncExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateTryTaskCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateTryTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateTaskExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateTryTaskExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateTryValueTaskCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateTryValueTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskExceptionBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateTryValueTaskExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    // Helper methods reintroduced
    private string GenerateTrySyncCall(ushort arity) {
        if (arity == 1) return "var transformedResult = result.Try<int, string>(x => x.ToString() + \"_tried\");";
        var paramsList = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_tried\""));
        var inputTypes = GenerateTypeParams(arity);
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity).Select(_ => "string"));
        return $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({paramsList}) => ({tupleItems}));";
    }
    private string GenerateTrySyncExceptionCall(ushort arity) {
        var inputTypes = GenerateTypeParams(arity);
        var outputTypes = arity == 1 ? "string" : string.Join(", ", Enumerable.Range(1, arity).Select(_ => "string"));
        return arity == 1
            ? "var transformedResult = result.Try<int, string>(x => throw new Exception(\"Boom\"));"
            : $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => throw new Exception(\"Boom\"));";
    }
    private string GenerateTryTaskCall(ushort arity) {
        return GenerateTryAsyncCall(arity, "Task")
              .Replace("result", "result")
              .Insert(0, "var result = await taskResult;\n");
    }

    private string GenerateTryAsyncCall(ushort arity, string asyncType) {
        if (arity == 1) return $"var transformedResult = await result.TryAsync<int, string>(x => {asyncType}.FromResult(x.ToString() + \"_tried\"));";
        var paramsList  = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"));
        var tupleItems  = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i} + \"_tried\""));
        var inputTypes  = GenerateTypeParams(arity);
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity).Select(_ => "string"));
        return $"var transformedResult = await result.TryAsync<{inputTypes}, {outputTypes}>(({paramsList}) => {asyncType}.FromResult(({tupleItems})));";
    }

    private string GenerateTryTaskExceptionCall(ushort  arity) => GenerateTryAsyncExceptionCall(arity).Replace("result", "result").Insert(0, "var result = await taskResult;\n");

    private string GenerateTryAsyncExceptionCall(ushort arity) {
        var inputTypes  = GenerateTypeParams(arity);
        var outputTypes = arity == 1 ? "string" : string.Join(", ", Enumerable.Range(1, arity).Select(_ => "string"));
        return arity == 1
                   ? "var transformedResult = await result.TryAsync<int, string>(x => throw new Exception(\"Boom\"));"
                   : $"var transformedResult = await result.TryAsync<{inputTypes}, {outputTypes}>(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => throw new Exception(\"Boom\"));";
    }

    private string GenerateTryValueTaskCall(ushort arity) => GenerateTryAsyncCall(arity, "ValueTask").Replace("result", "result").Insert(0, "var result = await valueTaskResult;\n");
    private string GenerateTryValueTaskExceptionCall(ushort arity) => GenerateTryAsyncExceptionCall(arity).Replace("result", "result").Insert(0, "var result = await valueTaskResult;\n");
    private string GenerateTrySuccessAssertions(ushort arity) => "Assert.True(transformedResult.IsSuccess);";
    private string GenerateTryExceptionAssertions(ushort arity) => "Assert.False(transformedResult.IsSuccess);";
}
