using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.FilterError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFilterErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultFilterErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFilterErrorTestsGenerator(string baseNamespace,
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
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultFilterErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync FilterError" };
        cw.AddMethod(new MethodWriter($"FilterError_Arity{arity}_Success_ShouldNotFilterError", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"FilterError_Arity{arity}_Failure_ShouldFilterError", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()],
                                      usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultFilterError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} FilterError" };
        cw.AddMethod(new MethodWriter($"FilterError{asyncType}_Arity{arity}_Success_ShouldNotFilterError", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"FilterError{asyncType}_Arity{arity}_Failure_ShouldFilterError", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var given = arity == 0
                        ? new[] { GenerateResultCreation(arity) }
                        : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateFilterErrorSyncCall(arity) };
        var then = new[] { "Assert.True(filteredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when = new[] { GenerateFilterErrorSyncCall(arity) };
        var then = new[] {
            "Assert.False(filteredResult.IsSuccess);", "Assert.Single(filteredResult.Errors);", "Assert.Equal(\"Test error\", filteredResult.Errors.First().Message);"
        };
        return BuildTestBody(given, when, then);
    }

    private string GenerateFilterErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = result.FilterError(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = result.FilterError<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }

    // Unified async bodies
    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var given = arity == 0
                        ? new[] { GenerateAsyncSuccessResultCreation(arity, asyncType) }
                        : new[] { GenerateTestValues(arity), GenerateAsyncSuccessResultCreation(arity, asyncType) };
        var when = new[] { GenerateFilterErrorAsyncCall(arity) };
        var then = new[] { "Assert.True(filteredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var given = new[] { GenerateAsyncFailureResultCreation(arity, asyncType) };
        var when = new[] { GenerateFilterErrorAsyncCall(arity) };
        var then = new[] {
            "Assert.False(filteredResult.IsSuccess);", "Assert.Single(filteredResult.Errors);", "Assert.Equal(\"Test error\", filteredResult.Errors.First().Message);"
        };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureResultCreation(ushort arity,
                                                      string asyncType)
    {
        string core;
        if (arity == 0)
        {
            core = "Result.Failure(\"Test error\")";
        }
        else
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(GetTestType));
            core = $"Result.Failure<{typeParams}>(\"Test error\")";
        }

        return $"var taskResult = {asyncType}.FromResult({core});";
    }

    private string GenerateFilterErrorAsyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = await taskResult.FilterErrorAsync(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = await taskResult.FilterErrorAsync<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }
}
