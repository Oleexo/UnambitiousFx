using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Recover extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultRecoverTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1; // Recover doesn't support arity 0 (no value to recover to)
    private const string ClassName = "ResultRecoverTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultRecoverTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultRecoverSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Recover" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncValueBasedSuccessTest(arity));
        cw.AddMethod(GenerateSyncValueBasedFailureTest(arity));
        cw.AddMethod(GenerateSyncDirectValueSuccessTest(arity));
        cw.AddMethod(GenerateSyncDirectValueFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultRecover{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Recover" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"Recover_Arity{arity}_Success_ShouldNotRecover",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"Recover_Arity{arity}_Failure_ShouldRecover",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"Recover{asyncType}_Arity{arity}_Success_ShouldNotRecover",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"Recover{asyncType}_Arity{arity}_Failure_ShouldRecover",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var given = new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateRecoverSyncCall(arity) };
        var then = new[] { "Assert.True(recoveredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var values = GenerateTestValues(arity);
        var given = new[] { values, GenerateFailureResultCreation(arity) };
        var when = new[] { GenerateRecoverSyncCall(arity) };
        var then = GenerateRecoverSuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var given = new[] { GenerateTestValues(arity), GenerateAsyncSuccessResultCreation(arity, asyncType) };
        var when = new[] { GenerateRecoverAsyncCall(arity, asyncType) };
        var then = new[] { "Assert.True(recoveredResult.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var given = new[] { GenerateAsyncFailureResultCreation(arity, asyncType) };
        var when = new[] { GenerateRecoverAsyncCall(arity, asyncType) };
        var then = GenerateRecoverSuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateRecoverAsyncCall(ushort arity,
                                            string asyncType)
    {
        var typeParams = GenerateTypeParams(arity);
        var recoveryValue = GetRecoveryValueExpression(arity);
        return $"var recoveredResult = await taskResult.RecoverAsync<{typeParams}>(errors => {asyncType}.FromResult({recoveryValue}));";
    }

    private string GenerateRecoverSyncCall(ushort arity)
    {
        var typeParams = GenerateTypeParams(arity);
        var recoveryValue = GenerateValueParams(arity, "value");
        return $"var recoveredResult = result.Recover<{typeParams}>(errors => ({recoveryValue}));";
    }

    private string GetRecoveryValueExpression(ushort arity)
    {
        if (arity == 1)
        {
            return "42";
        }

        var recoveryValues = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(GetOtherValue));
        return $"({recoveryValues})";
    }

    private string GenerateRecoverSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return """
                   Assert.True(recoveredResult.IsSuccess);
                   Assert.True(recoveredResult.TryGet(out var recoveredValue));
                   Assert.Equal(42, recoveredValue);
                   """;
        }

        return "Assert.True(recoveredResult.IsSuccess);";
    }

    #region Value-based Recover Tests

    private MethodWriter GenerateSyncValueBasedSuccessTest(ushort arity)
    {
        return new MethodWriter($"RecoverWithValues_Arity{arity}_Success_ShouldNotRecover",
                                "void",
                                GenerateSyncValueBasedSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncValueBasedFailureTest(ushort arity)
    {
        return new MethodWriter($"RecoverWithValues_Arity{arity}_Failure_ShouldRecover",
                                "void",
                                GenerateSyncValueBasedFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncValueBasedSuccessBody(ushort arity)
    {
        var given = new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateRecoverValuesSyncCall(arity) };
        var then = new[] { GenerateValueBasedSuccessAssertions(arity) };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncValueBasedFailureBody(ushort arity)
    {
        var given = new[] {
            GenerateTestValues(arity),
            GenerateFailureResultCreation(arity)
        };
        var when = new[] { GenerateRecoverValuesSyncCall(arity) };
        var then = GenerateValueBasedRecoverSuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateRecoverValuesSyncCall(ushort arity)
    {
        var typeParams = GenerateTypeParams(arity);
        var fallbackValues = GenerateValueParams(arity, "value");
        return $"var recoveredResult = result.Recover<{typeParams}>(errors => ({fallbackValues}));";
    }

    private string GenerateValueBasedSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return "Assert.True(recoveredResult.IsSuccess);\nAssert.True(recoveredResult.TryGet(out var recoveredValue));\nAssert.Equal(value1, recoveredValue);";
        }

        return "Assert.True(recoveredResult.IsSuccess);";
    }

    private string GenerateValueBasedRecoverSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return """
                   Assert.True(recoveredResult.IsSuccess);
                   Assert.True(recoveredResult.TryGet(out var recoveredValue));
                   Assert.Equal(42, recoveredValue);
                   """;
        }

        var assertions = new List<string> { "Assert.True(recoveredResult.IsSuccess);" };

        // Generate TryGet call with appropriate number of out parameters
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var recoveredValue{i}"));
        assertions.Add($"Assert.True(recoveredResult.TryGet({outParams}, out _));");

        // Generate assertions for each recovered value
        for (var i = 1; i <= arity; i++)
        {
            var testType = GetTestType(i);

            // Use Assert.False for boolean values to comply with xUnit2004
            if (testType == "bool")
            {
                assertions.Add($"Assert.True(recoveredValue{i});");
            }
            else
            {
                assertions.Add($"Assert.Equal(value{i}, recoveredValue{i});");
            }
        }

        return string.Join('\n', assertions);
    }

    #endregion

    #region Direct Value-based Recover Tests

    private MethodWriter GenerateSyncDirectValueSuccessTest(ushort arity)
    {
        return new MethodWriter($"RecoverWithDirectValues_Arity{arity}_Success_ShouldNotRecover",
                                "void",
                                GenerateSyncDirectValueSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncDirectValueFailureTest(ushort arity)
    {
        return new MethodWriter($"RecoverWithDirectValues_Arity{arity}_Failure_ShouldRecover",
                                "void",
                                GenerateSyncDirectValueFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncDirectValueSuccessBody(ushort arity)
    {
        var given = new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateRecoverDirectValuesSyncCall(arity) };
        var then = new[] { GenerateDirectValueSuccessAssertions(arity) };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncDirectValueFailureBody(ushort arity)
    {
        var given = new[] {
            GenerateTestValues(arity),
            GenerateFailureResultCreation(arity)
        };
        var when = new[] { GenerateRecoverDirectValuesSyncCall(arity) };
        var then = GenerateDirectValueRecoverSuccessAssertions(arity)
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody(given, when, then);
    }

    private string GenerateRecoverDirectValuesSyncCall(ushort arity)
    {
        if (arity == 1)
        {
            return "var recoveredResult = result.Recover(value1);";
        }

        var fallbackValues = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(i => $"value{i}"));
        return $"var recoveredResult = result.Recover({fallbackValues});";
    }

    private string GenerateDirectValueSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return "Assert.True(recoveredResult.IsSuccess);\nAssert.True(recoveredResult.TryGet(out var recoveredValue));\nAssert.Equal(value1, recoveredValue);";
        }

        var assertions = new List<string> { "Assert.True(recoveredResult.IsSuccess);" };

        // Generate TryGet call with appropriate number of out parameters
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var recoveredValue{i}"));
        assertions.Add($"Assert.True(recoveredResult.TryGet({outParams}, out _));");

        // Generate assertions for each value
        for (var i = 1; i <= arity; i++)
        {
            assertions.Add($"Assert.Equal(value{i}, recoveredValue{i});");
        }

        return string.Join('\n', assertions);
    }

    private string GenerateDirectValueRecoverSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return """
                   Assert.True(recoveredResult.IsSuccess);
                   Assert.True(recoveredResult.TryGet(out var recoveredValue));
                   Assert.Equal(value1, recoveredValue);
                   """;
        }

        var assertions = new List<string> { "Assert.True(recoveredResult.IsSuccess);" };

        // Generate TryGet call with appropriate number of out parameters
        var outParams = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"out var recoveredValue{i}"));
        assertions.Add($"Assert.True(recoveredResult.TryGet({outParams}, out _));");

        // Generate assertions for each recovered value
        for (var i = 1; i <= arity; i++)
        {
            assertions.Add($"Assert.Equal(value{i}, recoveredValue{i});");
        }

        return string.Join('\n', assertions);
    }

    #endregion
}
