using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.FindError extension methods (sync, Task, ValueTask).
///     Refactored for homogeneity using variant helper.
/// </summary>
internal sealed class ResultFindErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultFindErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFindErrorTestsGenerator(string baseNamespace,
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
        return GenerateVariants(arity,
                                ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultFindErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync FindError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultFindError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} FindError" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"FindError_Arity{arity}_Success_ShouldReturnNull",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"FindError_Arity{arity}_Failure_ShouldReturnError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"FindError{asyncType}_Arity{arity}_Success_ShouldReturnNull",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"FindError{asyncType}_Arity{arity}_Failure_ShouldReturnError",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = arity == 0
                             ? string.Empty
                             : GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateFindErrorSyncCall(arity);
        var givenLines = arity == 0
                             ? new[] { creation }
                             : new[] { testValues, creation };
        return BuildTestBody(givenLines,
                             [call],
                             ["Assert.Null(foundError);"]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateErrorTypeFailureResultCreation(arity);
        var call = GenerateFindErrorSyncCall(arity);
        return BuildTestBody([failureCreation],
                             [call],
                             ["Assert.NotNull(foundError);", "Assert.Equal(\"Test error\", foundError.Message);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = arity == 0
                             ? string.Empty
                             : GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateFindErrorAsyncCall(arity, asyncType);
        var givenLines = arity == 0
                             ? new[] { creation }
                             : new[] { testValues, creation };
        return BuildTestBody(givenLines,
                             [call],
                             ["Assert.Null(foundError);"]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateFindErrorAsyncCall(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.NotNull(foundError);", "Assert.Equal(\"Test error\", foundError.Message);"]);
    }

    private string GenerateFindErrorAsyncCall(ushort arity,
                                              string asyncType)
    {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        if (arity == 0)
        {
            return $"var foundError = await taskResult.FindErrorAsync(e => {asyncType}.FromResult(e.Message == \"Test error\"));";
        }

        return $"var foundError = await taskResult.FindErrorAsync<{typeParams}>(e => {asyncType}.FromResult(e.Message == \"Test error\"));";
    }

    private string GenerateFindErrorSyncCall(ushort arity)
    {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        if (arity == 0)
        {
            return "var foundError = result.FindError(e => e.Message == \"Test error\");";
        }

        return $"var foundError = result.FindError<{typeParams}>(e => e.Message == \"Test error\");";
    }
}
