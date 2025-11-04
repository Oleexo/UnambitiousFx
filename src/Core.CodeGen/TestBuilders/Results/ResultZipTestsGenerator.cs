using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultZipTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 2;
    private const string ClassName           = "ResultZipTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultZipTestsGenerator(string               baseNamespace,
                                   FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultZipSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Zip" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultZipTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Zip" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultZipValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Zip" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Zip_Arity{arity}_Success_ShouldZip", "void", GenerateSyncSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Zip_Arity{arity}_Failure_ShouldNotZip", "void", GenerateSyncFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"ZipTask_Arity{arity}_Success_ShouldZip", "async Task", GenerateTaskSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"ZipTask_Arity{arity}_Failure_ShouldNotZip", "async Task", GenerateTaskFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"ZipValueTask_Arity{arity}_Success_ShouldZip", "async Task", GenerateValueTaskSuccessBody(arity),
                                                                           attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"ZipValueTask_Arity{arity}_Failure_ShouldNotZip", "async Task", GenerateValueTaskFailureBody(arity),
                                                                           attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreations(arity);
        var call       = GenerateZipSyncCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation      = GenerateFailureResultCreations(arity);
        var call          = GenerateZipSyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(zippedResult.IsSuccess);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreations(arity);
        var call       = GenerateZipTaskCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreations(arity);
        var call     = GenerateZipTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(zippedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreations(arity);
        var call       = GenerateZipValueTaskCall(arity);
        var assertions = GenerateZipSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreations(arity);
        var call     = GenerateZipValueTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(zippedResult.IsSuccess);"]);
    }

    private string GenerateZipSyncCall(ushort arity) => arity == 2
                                                            ? "var zippedResult = result1.Zip(result2);"
                                                            : $"var zippedResult = result1.Zip({string.Join(", ", Enumerable.Range(2, arity - 1).Select(i => $"result{i}"))});";

    private string GenerateZipTaskCall(ushort arity) => arity == 2
                                                            ? "var zippedResult = await taskResult1.ZipAsync(taskResult2);"
                                                            : $"var zippedResult = await taskResult1.ZipAsync({string.Join(", ", Enumerable.Range(2, arity - 1).Select(i => $"taskResult{i}"))});";

    private string GenerateZipValueTaskCall(ushort arity) => arity == 2
                                                                 ? "var zippedResult = await valueTaskResult1.ZipAsync(valueTaskResult2);"
                                                                 : $"var zippedResult = await valueTaskResult1.ZipAsync({string.Join(", ", Enumerable.Range(2, arity - 1).Select(i => $"valueTaskResult{i}"))});";

    private string GenerateFailureTestValues(ushort arity) => GenerateTestValues(arity); // reuse

    private string GenerateResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                        .Select(i => $"var result{i} = Result.Success(value{i});"));

    private string GenerateFailureResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                               .Select(i => $"var result{i} = Result.Failure<{GetTestType(i)}>(\"Test error\");"));

    private string GenerateTaskResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                            .Select(i => $"var taskResult{i} = Task.FromResult(Result.Success(value{i}));"));

    private string GenerateTaskFailureResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                                   .Select(i =>
                                                                                                               $"var taskResult{i} = Task.FromResult(Result.Failure<{GetTestType(i)}>(\"Test error\"));"));

    private string GenerateValueTaskResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                                 .Select(i =>
                                                                                                             $"var valueTaskResult{i} = ValueTask.FromResult(Result.Success(value{i}));"));

    private string GenerateValueTaskFailureResultCreations(ushort arity) => string.Join('\n', Enumerable.Range(1, arity)
                                                                                                        .Select(i =>
                                                                                                                    $"var valueTaskResult{i} = ValueTask.FromResult(Result.Failure<{GetTestType(i)}>(\"Test error\"));"));

    private string GenerateZipSuccessAssertions(ushort arity) => "Assert.True(zippedResult.IsSuccess);";
}
