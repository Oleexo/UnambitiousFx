using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultZipTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 2;
    private const string ClassName = "ResultZipTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultZipTestsGenerator(string baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true))
    {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultZipSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Zip" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultZip{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Zip" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Zip_Arity{arity}_Success_ShouldZip", "void", GenerateSyncSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Zip_Arity{arity}_Failure_ShouldNotZip", "void", GenerateSyncFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) =>
        new($"Zip{asyncType}_Arity{arity}_Success_ShouldZip", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
            attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) =>
        new($"Zip{asyncType}_Arity{arity}_Failure_ShouldNotZip", "async Task", GenerateAsyncFailureBody(arity, asyncType),
            attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreations(arity);
        var call = GenerateZipSyncCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreations(arity);
        var call = GenerateZipSyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(zippedResult.IsSuccess);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncResultCreations(arity, asyncType);
        var call = GenerateZipAsyncCall(arity, asyncType);
        var assertions = GenerateZipSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreations(arity, asyncType);
        var call = GenerateZipAsyncCall(arity, asyncType);
        return BuildTestBody([creation], [call], ["Assert.False(zippedResult.IsSuccess);"]);
    }

    private string GenerateAsyncResultCreations(ushort arity,
                                                string asyncType)
    {
        var varPrefix = $"{asyncType.ToLowerInvariant()}Result";
        return string.Join('\n', Enumerable.Range(1, arity)
                                           .Select(i => $"var {varPrefix}{i} = {asyncType}.FromResult(Result.Success(value{i}));"));
    }

    private string GenerateAsyncFailureResultCreations(ushort arity,
                                                       string asyncType)
    {
        var varPrefix = $"{asyncType.ToLowerInvariant()}Result";
        return string.Join('\n', Enumerable.Range(1, arity)
                                           .Select(i => $"var {varPrefix}{i} = {asyncType}.FromResult(Result.Failure<{GetTestType(i)}>(\"Test error\"));"));
    }

    private string GenerateZipAsyncCall(ushort arity,
                                        string asyncType)
    {
        var varPrefix = $"{asyncType.ToLowerInvariant()}Result";
        if (arity == 2)
        {
            return $"var zippedResult = await {varPrefix}1.ZipAsync({varPrefix}2);";
        }
        var args = string.Join(", ", Enumerable.Range(2, arity - 1).Select(i => $"{varPrefix}{i}"));
        return $"var zippedResult = await {varPrefix}1.ZipAsync({args});";
    }

    private string GenerateZipSyncCall(ushort arity) => arity == 2
                                                            ? "var zippedResult = result1.Zip(result2);"
                                                            : $"var zippedResult = result1.Zip({string.Join(", ", Enumerable.Range(2, arity - 1).Select(i => $"result{i}"))});";

    private string GenerateFailureTestValues(ushort arity) => GenerateTestValues(arity); // reuse

    private string GenerateResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                        .Select(i => $"var result{i} = Result.Success(value{i});"));

    private string GenerateFailureResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                               .Select(i => $"var result{i} = Result.Failure<{GetTestType(i)}>(\"Test error\");"));


    private string GenerateZipSuccessAssertions(ushort arity) => "Assert.True(zippedResult.IsSuccess);";
}
