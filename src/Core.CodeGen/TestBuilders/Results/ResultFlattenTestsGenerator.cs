using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Flatten extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFlattenTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultFlattenTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultFlattenTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultFlattenSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Flatten" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultFlattenTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Flatten" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"Flatten_Arity{arity}_Success_ShouldFlatten",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"Flatten_Arity{arity}_Failure_ShouldNotFlatten",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"FlattenTask_Arity{arity}_Success_ShouldFlatten",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"FlattenTask_Arity{arity}_Failure_ShouldNotFlatten",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateNestedResultCreation(arity);
        var call = "var flattenedResult = nestedResult.Flatten();";
        var assertions = GenerateFlattenSuccessAssertions();
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureNestedResultCreation(arity);
        var call = "var flattenedResult = nestedResult.Flatten();";
        return BuildTestBody([creation], [call], ["Assert.False(flattenedResult.IsSuccess);"]);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessNestedResultCreation(arity, asyncType);
        var call = "var flattenedResult = await taskNestedResult.FlattenAsync();";
        var assertions = GenerateFlattenSuccessAssertions();
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureNestedResultCreation(arity,
                                                                asyncType);
        var call = "var flattenedResult = await taskNestedResult.FlattenAsync();";
        return BuildTestBody([creation], [call], ["Assert.False(flattenedResult.IsSuccess);"]);
    }

    private string GenerateSuccessSuccessResultCreation(ushort arity)
    {
        if (arity == 1)
        {
            return "Result.Success(Result.Success(value1))";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"value{i}"));
        return $"Result.Success(Result.Success({values}))";
    }

    private string GenerateNestedResultCreation(ushort arity)
    {
        return $"var nestedResult = {GenerateSuccessSuccessResultCreation(arity)};";
    }

    private string GenerateSuccessFailureResultCreation(ushort arity)
    {
        var typeParams = GenerateTypeParams(arity);
        return $"Result.Success(Result.Failure<{typeParams}>(\"Test error\"))";
    }

    private string GenerateFailureNestedResultCreation(ushort arity)
    {
        return $"var nestedResult = {GenerateSuccessFailureResultCreation(arity)};";
    }

    private string GenerateAsyncSuccessNestedResultCreation(ushort arity,
                                                            string asyncType)
    {
        var core = GenerateSuccessSuccessResultCreation(arity);
        return $"var taskNestedResult = {asyncType}.FromResult({core});";
    }

    private string GenerateAsyncFailureNestedResultCreation(ushort arity,
                                                            string asyncType)
    {
        var core = GenerateSuccessFailureResultCreation(arity);
        return $"var taskNestedResult = {asyncType}.FromResult({core});";
    }

    private string GenerateFlattenSuccessAssertions()
    {
        return "Assert.True(flattenedResult.IsSuccess);";
    }
}
