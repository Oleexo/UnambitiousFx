using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Try extension methods (sync, Task, ValueTask) including exception path.
/// </summary>
internal sealed class ResultTryTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultTryTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultTryTestsGenerator(string baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        return GenerateVariants(arity, ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
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

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultTry{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Try" };
        cw.AddMethod(new MethodWriter($"Try{asyncType}_Arity{arity}_Success_ShouldTransform",
                                      "async Task",
                                      GenerateAsyncSuccessBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Try{asyncType}_Arity{arity}_Failure_ShouldNotTransform",
                                      "async Task",
                                      GenerateAsyncFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"Try{asyncType}_Arity{arity}_Exception_ShouldReturnExceptionalError",
                                      "async Task",
                                      GenerateAsyncExceptionBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateTrySyncCall(arity);
        var assertions = GenerateTrySuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateTrySyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateSyncExceptionBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateTrySyncExceptionCall(arity);
        var assertions = GenerateTryExceptionAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateTryAsyncCall(arity, asyncType);
        var assertions = GenerateTrySuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation], [call], assertions);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateTryAsyncCall(arity, asyncType);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateAsyncExceptionBody(ushort arity,
                                              string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var tryCallCore = GenerateTryAsyncCallCoreCreation(arity, asyncType);
        var call = GenerateTryAsyncExceptionCall(arity, asyncType);
        var assertions = GenerateTryExceptionAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, tryCallCore], [call], assertions);
    }

    private string GenerateTryAsyncCallCoreCreation(ushort arity,
                                                    string asyncType)
    {
        if (arity == 1)
        {
            return $"Func<int, {asyncType}<string>> tryCall = (value1) => {{ throw new InvalidOperationException(\"Test exception\"); }};";
        }

        var typeParams = GenerateTypeParams(arity);
        var paramsList = GenerateValueParams(arity, "x");
        return $"Func<{typeParams}, {asyncType}<({typeParams})>> tryCall = ({paramsList}) => {{ throw new InvalidOperationException(\"Test exception\"); }};";
    }

    private string GenerateTryAsyncCall(ushort arity,
                                        string asyncType)
    {
        if (arity == 1)
        {
            return $"var transformedResult = await taskResult.TryAsync<int, string>(x => {asyncType}.FromResult(x.ToString() + \"_tried\"));";
        }

        var paramsList = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        return $"var transformedResult = await taskResult.TryAsync<{typeParams}, {typeParams}>(({paramsList}) => {asyncType}.FromResult(({paramsList})));";
    }

    private string GenerateTryAsyncExceptionCall(ushort arity,
                                                 string asyncType)
    {
        if (arity == 1)
        {
            return "var transformedResult = await taskResult.TryAsync<int, string>(tryCall);";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var transformedResult = await taskResult.TryAsync<{typeParams}, {typeParams}>(tryCall);";
    }

    // Helper methods reintroduced
    private string GenerateTrySyncCall(ushort arity)
    {
        if (arity == 1)
        {
            return "var transformedResult = result.Try<int, string>(x => x.ToString() + \"_tried\");";
        }

        var paramsList = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var tupleItems = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i} + \"_tried\""));
        var inputTypes = GenerateTypeParams(arity);
        var outputTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                      .Select(_ => "string"));
        return $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({paramsList}) => ({tupleItems}));";
    }

    private string GenerateTrySyncExceptionCall(ushort arity)
    {
        var inputTypes = GenerateTypeParams(arity);
        var outputTypes = arity == 1
                              ? "string"
                              : string.Join(", ", Enumerable.Range(1, arity)
                                                            .Select(_ => "string"));
        return arity == 1
                   ? "var transformedResult = result.Try<int, string>(x => throw new Exception(\"Boom\"));"
                   : $"var transformedResult = result.Try<{inputTypes}, {outputTypes}>(({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"x{i}"))}) => throw new Exception(\"Boom\"));";
    }

    private string GenerateTrySuccessAssertions(ushort arity)
    {
        return "Assert.True(transformedResult.IsSuccess);";
    }

    private string GenerateTryExceptionAssertions(ushort arity)
    {
        return "Assert.False(transformedResult.IsSuccess);";
    }
}
