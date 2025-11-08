using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.AppendError extension methods (sync, Task, ValueTask, direct async).
///     Refactored for homogeneity using shared variant generation helper.
/// </summary>
internal sealed class ResultAppendErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Includes non-generic Result
    private const string ClassName = "ResultAppendErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultAppendErrorTestsGenerator(string baseNamespace,
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
                                (x => GenerateAwaitableAsyncTests(x, "Task"), true),
                                (x => GenerateAwaitableAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultAppendErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync AppendError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAwaitableAsyncTests(ushort arity,
                                                    string asyncType)
    {
        var cw = new ClassWriter($"ResultAppendError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} AppendError" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity));
        cw.AddMethod(GenerateAsyncFailureTest(arity));
        cw.AddMethod(GenerateAwaitableAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAwaitableAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"AppendError_Arity{arity}_Success_ShouldNotAppendError",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"AppendError_Arity{arity}_Failure_ShouldAppendError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAwaitableAsyncSuccessTest(ushort arity,
                                                           string asyncType)
    {
        return new MethodWriter($"AppendError{asyncType}_Arity{arity}_Success_ShouldNotAppendError",
                                "async Task",
                                GenerateAwaitableAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAwaitableAsyncFailureTest(ushort arity,
                                                           string asyncType)
    {
        return new MethodWriter($"AppendError{asyncType}_Arity{arity}_Failure_ShouldAppendError",
                                "async Task",
                                GenerateAwaitableAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"AppendErrorAsync_Arity{arity}_Success_ShouldNotAppendError",
                                "async Task",
                                GenerateAsyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity)
    {
        return new MethodWriter($"AppendErrorAsync_Arity{arity}_Failure_ShouldAppendError",
                                "async Task",
                                GenerateAsyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GenerateAppendErrorSyncCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateAppendErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateAwaitableAsyncSuccessBody(ushort arity,
                                                     string asyncType)
    {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateAppendErrorAwaitableAsyncCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateAwaitableAsyncFailureBody(ushort arity,
                                                     string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateAppendErrorAwaitableAsyncCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateAsyncSuccessBody(ushort arity)
    {
        var testValues = arity > 0
                             ? GenerateTestValues(arity)
                             : string.Empty;
        var creation = GenerateResultCreation(arity);
        var call = GenerateAppendErrorDirectAsyncCall(arity);
        if (arity == 0)
        {
            return $"""
                    // Given
                    {creation}
                    // When
                    {call}
                    // Then
                    Assert.True(appendedResult.IsSuccess);
                    """;
        }

        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(appendedResult.IsSuccess);
                """;
    }

    private string GenerateAsyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateAppendErrorDirectAsyncCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(appendedResult.IsSuccess);
                Assert.Single(appendedResult.Errors);
                Assert.Contains("Appended error", appendedResult.Errors.First().Message);
                Assert.StartsWith("Test error", appendedResult.Errors.First().Message);
                """;
    }

    private string GenerateAppendErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var appendedResult = result.AppendError(\"Appended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = result.AppendError<{typeParams}>(\"Appended error\");";
    }

    private string GenerateAppendErrorAwaitableAsyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var appendedResult = await taskResult.AppendErrorAsync(\"Appended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = await taskResult.AppendErrorAsync<{typeParams}>(\"Appended error\");";
    }

    private string GenerateAppendErrorDirectAsyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var appendedResult = await result.AppendErrorAsync(\"Appended error\");";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var appendedResult = await result.AppendErrorAsync<{typeParams}>(\"Appended error\");";
    }
}
