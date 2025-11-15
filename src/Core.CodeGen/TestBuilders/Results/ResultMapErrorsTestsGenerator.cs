using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MapErrors extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMapErrorsTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultMapErrorsTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorsTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultMapErrorsSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MapErrors" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultMapErrors{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} MapErrors" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrors_Arity{arity}_Success_ShouldNotMapErrors", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrors_Arity{arity}_Failure_ShouldMapErrors", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"MapErrors{asyncType}_Arity{arity}_Success_ShouldNotMapErrors", "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"MapErrors{asyncType}_Arity{arity}_Failure_ShouldMapErrors", "async Task",
                                GenerateAsyncFailureBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateMapErrorsSyncCall(arity);
        return BuildTestBody([testValues, creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateMapErrorsSyncCall(arity);
        return BuildTestBody([failureCreation],
                             [call],
                             ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateMapErrorsAsyncCall(arity, asyncType);
        return BuildTestBody([testValues, creation], [call], ["Assert.True(mappedResult.IsSuccess);"]);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateMapErrorsAsyncCall(arity, asyncType);
        return BuildTestBody([creation],
                             [call],
                             ["Assert.False(mappedResult.IsSuccess);", "Assert.Contains(\"MAPPED\", mappedResult.Errors.First().Message);"]);
    }

    private string GenerateMapErrorsSyncCall(ushort arity)
    {
        return arity == 0
                   ? "var mappedResult = result.MapErrors(errors => new Error(errors.First().Message + \" MAPPED\"));"
                   : $"var mappedResult = result.MapErrors<{GenerateTypeParams(arity)}>(errors => new Error(errors.First().Message + \" MAPPED\"));";
    }

    private string GenerateMapErrorsAsyncCall(ushort arity,
                                              string asyncType)
    {
        return arity == 0
                   ? $"var mappedResult = await taskResult.MapErrorsAsync(errors => {asyncType}.FromResult<IError>(new Error(errors.First().Message + \" MAPPED\")));"
                   : $"var mappedResult = await taskResult.MapErrorsAsync<{GenerateTypeParams(arity)}>(errors => {asyncType}.FromResult<IError>(new Error(errors.First().Message + \" MAPPED\")));";
    }
}
