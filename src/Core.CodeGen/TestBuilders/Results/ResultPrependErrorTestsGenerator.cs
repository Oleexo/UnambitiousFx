using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.PrependError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultPrependErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultPrependErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultPrependErrorTestsGenerator(string baseNamespace,
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
        var classes = new List<ClassWriter>();
        var sync = GenerateSyncTests(arity);
        if (sync != null)
        {
            classes.Add(sync);
        }

        var async = GenerateAsyncTests(arity);
        if (async != null)
        {
            async.UnderClass = ClassName;
            classes.Add(async);
        }

        var task = GenerateTaskTests(arity);
        if (task != null)
        {
            task.UnderClass = ClassName;
            classes.Add(task);
        }

        var valueTask = GenerateValueTaskTests(arity);
        if (valueTask != null)
        {
            valueTask.UnderClass = ClassName;
            classes.Add(valueTask);
        }

        return classes;
    }

    private ClassWriter? GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultPrependErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync PrependError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultPrependErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task PrependError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultPrependErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask PrependError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private ClassWriter? GenerateAsyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultPrependErrorAsyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Async PrependErrorAsync on Result" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity));
        cw.AddMethod(GenerateAsyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"PrependError_Arity{arity}_Success_ShouldNotPrependError",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"PrependError_Arity{arity}_Failure_ShouldPrependError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorTask_Arity{arity}_Success_ShouldNotPrependError",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorTask_Arity{arity}_Failure_ShouldPrependError",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorValueTask_Arity{arity}_Success_ShouldNotPrependError",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorValueTask_Arity{arity}_Failure_ShouldPrependError",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorAsync_Arity{arity}_Success_ShouldNotPrependError",
                                "async Task",
                                GenerateAsyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity)
    {
        return new MethodWriter($"PrependErrorAsync_Arity{arity}_Failure_ShouldPrependError",
                                "async Task",
                                GenerateAsyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GeneratePrependErrorSyncCall(arity);

        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(prependedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(prependedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GeneratePrependErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(prependedResult.IsSuccess);
                Assert.Single(prependedResult.Errors);
                Assert.Contains("Prepended error", prependedResult.Errors.First().Message);
                Assert.EndsWith("Test error", prependedResult.Errors.First().Message);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GeneratePrependErrorTaskCall(arity);

        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(prependedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(prependedResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GeneratePrependErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(prependedResult.IsSuccess);
                Assert.Single(prependedResult.Errors);
                Assert.Contains("Prepended error", prependedResult.Errors.First().Message);
                Assert.EndsWith("Test error", prependedResult.Errors.First().Message);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GeneratePrependErrorValueTaskCall(arity);

        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(prependedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(prependedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GeneratePrependErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(prependedResult.IsSuccess);
                Assert.Single(prependedResult.Errors);
                Assert.Contains("Prepended error", prependedResult.Errors.First().Message);
                Assert.EndsWith("Test error", prependedResult.Errors.First().Message);
                """;
    }

    private string GenerateAsyncSuccessBody(ushort arity)
    {
        var testValues = arity > 0 ? GenerateTestValues(arity) : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GeneratePrependErrorAsyncCall(arity);

        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(prependedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(prependedResult.IsSuccess);
                """;
    }

    private string GenerateAsyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GeneratePrependErrorAsyncCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(prependedResult.IsSuccess);
                Assert.Single(prependedResult.Errors);
                Assert.Contains("Prepended error", prependedResult.Errors.First().Message);
                Assert.EndsWith("Test error", prependedResult.Errors.First().Message);
                """;
    }

    private string GeneratePrependErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var prependedResult = result.PrependError(\"Prepended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var prependedResult = result.PrependError<{typeParams}>(\"Prepended error\");";
    }

    private string GeneratePrependErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var prependedResult = await Task.FromResult(result).PrependErrorAsync(\"Prepended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var prependedResult = await Task.FromResult(result).PrependErrorAsync<{typeParams}>(\"Prepended error\");";
    }

    private string GeneratePrependErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var prependedResult = await ValueTask.FromResult(result).PrependErrorAsync(\"Prepended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var prependedResult = await ValueTask.FromResult(result).PrependErrorAsync<{typeParams}>(\"Prepended error\");";
    }

    private string GeneratePrependErrorAsyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var prependedResult = await UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks.ResultExtensions.PrependErrorAsync(result, \"Prepended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var prependedResult = await UnambitiousFx.Core.Results.Extensions.ErrorHandling.ValueTasks.ResultExtensions.PrependErrorAsync<{typeParams}>(result, \"Prepended error\");";
    }
}


