using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Represents the type of test being generated (Sync, Task, or ValueTask).
/// </summary>
internal enum TestType
{
    Sync,
    Task,
    ValueTask
}

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
        var classes = new List<ClassWriter>();
        var sync = GenerateSyncTests(arity);
        if (sync != null)
        {
            classes.Add(sync);
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
        return GenerateTestClass(
            $"ResultRecoverSyncTestsArity{arity}",
            $"Arity {arity} - Sync Recover",
            $"{Config.BaseNamespace}.{ExtensionsNamespace}",
            arity,
            TestType.Sync);
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        return GenerateTestClass(
            $"ResultRecoverTaskTestsArity{arity}",
            $"Arity {arity} - Task Recover",
            $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks",
            arity,
            TestType.Task);
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        return GenerateTestClass(
            $"ResultRecoverValueTaskTestsArity{arity}",
            $"Arity {arity} - ValueTask Recover",
            $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks",
            arity,
            TestType.ValueTask);
    }

    private ClassWriter GenerateTestClass(string className, string region, string namespaceName, ushort arity, TestType testType)
    {
        var cw = new ClassWriter(className, Visibility.Public) { Region = region };
        cw.AddMethod(GenerateSuccessTest(arity, testType));
        cw.AddMethod(GenerateFailureTest(arity, testType));
        cw.Namespace = namespaceName;
        return cw;
    }

    private MethodWriter GenerateSuccessTest(ushort arity, TestType testType)
    {
        var (methodPrefix, returnType) = GetMethodInfo(testType);
        return new MethodWriter($"{methodPrefix}_Arity{arity}_Success_ShouldNotRecover",
                                returnType,
                                GenerateSuccessBody(arity, testType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateFailureTest(ushort arity, TestType testType)
    {
        var (methodPrefix, returnType) = GetMethodInfo(testType);
        return new MethodWriter($"{methodPrefix}_Arity{arity}_Failure_ShouldRecover",
                                returnType,
                                GenerateFailureBody(arity, testType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private (string methodPrefix, string returnType) GetMethodInfo(TestType testType)
    {
        return testType switch
        {
            TestType.Sync => ("Recover", "void"),
            TestType.Task => ("RecoverTask", "async Task"),
            TestType.ValueTask => ("RecoverValueTask", "async Task"),
            _ => throw new ArgumentOutOfRangeException(nameof(testType))
        };
    }

    private string GenerateSuccessBody(ushort arity, TestType testType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GetResultCreation(arity, testType);
        var call = GenerateRecoverCall(arity, testType);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(recoveredResult.IsSuccess);
                """;
    }

    private string GenerateFailureBody(ushort arity, TestType testType)
    {
        var failureCreation = GetFailureResultCreation(arity, testType);
        var call = GenerateRecoverCall(arity, testType);
        var assertions = GenerateRecoverSuccessAssertions(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                {assertions}
                """;
    }

    private string GetResultCreation(ushort arity, TestType testType)
    {
        return testType switch
        {
            TestType.Sync => GenerateResultCreation(arity),
            TestType.Task => GenerateTaskResultCreation(arity),
            TestType.ValueTask => GenerateValueTaskResultCreation(arity),
            _ => throw new ArgumentOutOfRangeException(nameof(testType))
        };
    }

    private string GetFailureResultCreation(ushort arity, TestType testType)
    {
        return testType switch
        {
            TestType.Sync => GenerateFailureResultCreation(arity),
            TestType.Task => GenerateTaskFailureResultCreation(arity),
            TestType.ValueTask => GenerateValueTaskFailureResultCreation(arity),
            _ => throw new ArgumentOutOfRangeException(nameof(testType))
        };
    }

    private string GenerateRecoverCall(ushort arity, TestType testType)
    {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType));
        var recoveryValue = GetRecoveryValueExpression(arity);

        return testType switch
        {
            TestType.Sync => $"var recoveredResult = result.Recover<{typeParams}>(errors => {recoveryValue});",
            TestType.Task => $"var recoveredResult = await taskResult.RecoverAsync<{typeParams}>(errors => Task.FromResult({recoveryValue}));",
            TestType.ValueTask => $"var recoveredResult = await valueTaskResult.RecoverAsync<{typeParams}>(errors => ValueTask.FromResult({recoveryValue}));",
            _ => throw new ArgumentOutOfRangeException(nameof(testType))
        };
    }

    private string GetRecoveryValueExpression(ushort arity)
    {
        if (arity == 1)
        {
            return "999";
        }

        var recoveryValues = string.Join(", ", Enumerable.Range(1, arity).Select(GetRecoveryValue));
        return $"({recoveryValues})";
    }

    private string GenerateRecoverSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return """
                   Assert.True(recoveredResult.IsSuccess);
                   Assert.True(recoveredResult.TryGet(out var recoveredValue));
                   Assert.Equal(999, recoveredValue);
                   """;
        }

        return "Assert.True(recoveredResult.IsSuccess);";
    }

    private string GetRecoveryValue(int index)
    {
        return index switch { 1 => "999", 2 => "\"recovered\"", 3 => "false", 4 => "9.99", 5 => "888L", _ => $"\"recovered{index}\"" };
    }
}
