using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultValueOrTestsGenerator : ResultTestGeneratorBase
{
    // base changed
    private const int StartArity = 1;
    private const string ClassName = "ResultValueOrTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrTestsGenerator(string baseNamespace,
                                       FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true))
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
        var cw = new ClassWriter($"ResultValueOrSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOr" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncSuccessWithFactoryTest(arity));
        cw.AddMethod(GenerateSyncFailureWithFactoryTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultValueOr{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} ValueOr" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncSuccessWithFactoryTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureWithFactoryTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ValueAccess.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"ValueOr_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"ValueOr_Arity{arity}_Failure_ShouldReturnFallback", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity)
    {
        return new MethodWriter($"ValueOrWithFactory_Arity{arity}_Success_ShouldReturnValue", "void",
                                GenerateSyncSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity)
    {
        return new MethodWriter($"ValueOrWithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue", "void",
                                GenerateSyncFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"ValueOr{asyncType}_Arity{arity}_Success_ShouldReturnValue", "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"ValueOr{asyncType}_Arity{arity}_Failure_ShouldReturnFallback", "async Task",
                                GenerateAsyncFailureBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessWithFactoryTest(ushort arity,
                                                             string asyncType)
    {
        return new MethodWriter($"ValueOr{asyncType}WithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task",
                                GenerateAsyncSuccessWithFactoryBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureWithFactoryTest(ushort arity,
                                                             string asyncType)
    {
        return new MethodWriter($"ValueOr{asyncType}WithFactory_Arity{arity}_Failure_ShouldReturnFactoryValue", "async Task",
                                GenerateAsyncFailureWithFactoryBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call = GenerateValueOrSyncCall(arity);
        var assertions = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var fallbackValues = GenerateFallbackValues(arity);
        var call = GenerateValueOrSyncCall(arity);
        var assertions = GenerateValueOrFailureAssertions(arity);
        return BuildTestBody([failureCreation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call = GenerateValueOrSyncWithFactoryCall(arity);
        var assertions = GenerateValueOrFactoryFailureAssertions(arity);
        return BuildTestBody([failureCreation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var fallbackValues = GenerateFallbackValues(arity);
        var call = GenerateValueOrAsyncCall(arity);
        var assertions = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var fallbackValues = GenerateFallbackValues(arity);
        var call = GenerateValueOrAsyncCall(arity);
        var assertions = GenerateValueOrFailureAssertions(arity);
        return BuildTestBody([creation, fallbackValues], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncSuccessWithFactoryBody(ushort arity,
                                                       string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call = GenerateValueOrAsyncWithFactoryCall(arity);
        var assertions = GenerateValueOrSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureWithFactoryBody(ushort arity,
                                                       string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var factoryDefinition = GenerateFactoryDefinition(arity);
        var call = GenerateValueOrAsyncWithFactoryCall(arity);
        var assertions = GenerateValueOrFactoryFailureAssertions(arity);
        return BuildTestBody([creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueOrSyncCall(ushort arity)
    {
        return arity == 1
                   ? "var actualValue = result.ValueOr(fallback1);"
                   : $"var actualValue = result.ValueOr({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))});";
    }

    private string GenerateValueOrSyncWithFactoryCall(ushort arity)
    {
        return "var actualValue = result.ValueOr(factory);";
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

    private string GenerateValueOrAsyncCall(ushort arity)
    {
        return arity == 1
                   ? "var actualValue = await taskResult.ValueOrAsync(fallback1);"
                   : $"var actualValue = await taskResult.ValueOrAsync({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))});";
    }

    private string GenerateValueOrAsyncWithFactoryCall(ushort arity)
    {
        return "var actualValue = await taskResult.ValueOrAsync(factory);";
    }

    private string GenerateValueOrSuccessAssertions(ushort arity)
    {
        return arity == 1
                   ? "Assert.Equal(value1, actualValue);"
                   : string.Join('\n', Enumerable.Range(1, arity)
                                                 .Select(i => $"Assert.Equal(value{i}, actualValue.Item{i});"));
    }

    private string GenerateValueOrFailureAssertions(ushort arity)
    {
        return arity == 1
                   ? "Assert.Equal(fallback1, actualValue);"
                   : string.Join('\n', Enumerable.Range(1, arity)
                                                 .Select(i => $"Assert.Equal(fallback{i}, actualValue.Item{i});"));
    }

    private string GenerateValueOrFactoryFailureAssertions(ushort arity)
    {
        return GenerateValueOrFailureAssertions(arity);
    }

    private string GenerateFallbackValues(ushort arity)
    {
        return string.Join('\n', Enumerable.Range(1, arity)
                                           .Select(i => $"var fallback{i} = {GetOtherValue(i)};"));
    }

    private string GenerateFactoryDefinition(ushort arity)
    {
        if (arity == 1)
        {
            return "var fallback1 = 999;\nFunc<int> factory = () => fallback1;";
        }

        var fallbackDecls = string.Join('\n', Enumerable.Range(1, arity)
                                                        .Select(i => $"var fallback{i} = {GetOtherValue(i)};"));
        var tupleType = $"({string.Join(", ", Enumerable.Range(1, arity).Select(GetTestType))})";
        var tupleValues = $"({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"fallback{i}"))})";
        return $"{fallbackDecls}\nFunc<{tupleType}> factory = () => {tupleValues};";
    }
}
