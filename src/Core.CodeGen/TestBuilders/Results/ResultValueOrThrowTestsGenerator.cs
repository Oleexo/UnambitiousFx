using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultValueOrThrowTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultValueOrThrowTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultValueOrThrowTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultValueOrThrowSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ValueOrThrow" };
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
        var cw = new ClassWriter($"ResultValueOrThrow{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} ValueOrThrow" };
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
        return new MethodWriter($"ValueOrThrow_Arity{arity}_Success_ShouldReturnValue", "void", GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"ValueOrThrow_Arity{arity}_Failure_ShouldThrowException", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncSuccessWithFactoryTest(ushort arity)
    {
        return new MethodWriter($"ValueOrThrowWithFactory_Arity{arity}_Success_ShouldReturnValue", "void",
                                GenerateSyncSuccessWithFactoryBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureWithFactoryTest(ushort arity)
    {
        return new MethodWriter($"ValueOrThrowWithFactory_Arity{arity}_Failure_ShouldThrowCustomException", "void",
                                GenerateSyncFailureWithFactoryBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"ValueOrThrow{asyncType}_Arity{arity}_Success_ShouldReturnValue", "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"ValueOrThrow{asyncType}_Arity{arity}_Failure_ShouldThrowException", "async Task",
                                GenerateAsyncFailureBody(arity, asyncType), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessWithFactoryTest(ushort arity,
                                                             string asyncType)
    {
        return new MethodWriter($"ValueOrThrow{asyncType}WithFactory_Arity{arity}_Success_ShouldReturnValue", "async Task",
                                GenerateAsyncSuccessWithFactoryBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureWithFactoryTest(ushort arity,
                                                             string asyncType)
    {
        return new MethodWriter($"ValueOrThrow{asyncType}WithFactory_Arity{arity}_Failure_ShouldThrowCustomException",
                                "async Task", GenerateAsyncFailureWithFactoryBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateValueOrThrowSyncCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateValueOrThrowSyncCall(arity);
        var expr = call.Replace("var actualValue = ", string.Empty)
                       .TrimEnd(';');
        return BuildTestBody([failureCreation], ["// When & Then", $"Assert.Throws<Exception>(() => {expr});"], []);
    }

    private string GenerateSyncSuccessWithFactoryBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call = GenerateValueOrThrowSyncWithFactoryCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureWithFactoryBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call = GenerateValueOrThrowSyncWithFactoryCall(arity);
        var expr = call.Replace("var actualValue = ", string.Empty)
                       .TrimEnd(';');
        return BuildTestBody([failureCreation, factoryDefinition], [$"Assert.Throws<Exception>(() => {expr});"], []);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call = GenerateValueOrThrowAsyncCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call = GenerateValueOrThrowAsyncCall(arity);
        var expr = call.Replace("var actualValue = ", string.Empty)
                       .Replace("await ", string.Empty)
                       .TrimEnd(';');
        return BuildTestBody([creation], ["await Assert.ThrowsAsync<Exception>(async () => {", $"    await {expr};", "});"], []);
    }

    private string GenerateAsyncSuccessWithFactoryBody(ushort arity,
                                                       string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call = GenerateValueOrThrowAsyncWithFactoryCall(arity);
        var assertions = GenerateValueOrThrowSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, factoryDefinition], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureWithFactoryBody(ushort arity,
                                                       string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var factoryDefinition = GenerateExceptionFactoryDefinition();
        var call = GenerateValueOrThrowAsyncWithFactoryCall(arity);
        var expr = call.Replace("var actualValue = ", string.Empty)
                       .Replace("await ", string.Empty)
                       .TrimEnd(';');
        return BuildTestBody([creation, factoryDefinition], ["await Assert.ThrowsAsync<Exception>(async () => {", $"    await {expr};", "});"], []);
    }

    private string GenerateValueOrThrowSyncCall(ushort arity)
    {
        return arity == 1
                   ? "var actualValue = result.ValueOrThrow();"
                   : "var actualValue = result.ValueOrThrow();";
        // multi-arity returns tuple implicitly
    }

    private string GenerateValueOrThrowSyncWithFactoryCall(ushort arity)
    {
        return arity == 1
                   ? "var actualValue = result.ValueOrThrow(factory);"
                   : "var actualValue = result.ValueOrThrow(factory);";
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

    private string GenerateValueOrThrowAsyncCall(ushort arity)
    {
        return "var actualValue = await taskResult.ValueOrThrowAsync();";
    }

    private string GenerateValueOrThrowAsyncWithFactoryCall(ushort arity)
    {
        return "var actualValue = await taskResult.ValueOrThrowAsync(factory);";
    }

    private string GenerateValueOrThrowSuccessAssertions(ushort arity)
    {
        if (arity == 1)
        {
            return "Assert.Equal(value1, actualValue);";
        }

        static string AccessPath(int index)
        {
            if (index <= 7)
            {
                return $"actualValue.Item{index}";
            }

            var restCount = (index - 1) / 7;
            var within = index - restCount * 7;
            var restChain = string.Concat(Enumerable.Repeat(".Rest", restCount));
            return $"actualValue{restChain}.Item{within}";
        }

        var lines = Enumerable.Range(1, arity)
                              .Select(i => $"Assert.Equal(value{i}, {AccessPath(i)});");
        return string.Join('\n', lines);
    }

    private string GenerateExceptionFactoryDefinition()
    {
        return "Func<IEnumerable<IError>, Exception> factory = (_) => new Exception();";
    }

    private string GenerateFailureResultCreation(ushort arity)
    {
        var typeParams = GenerateTypeParams(arity);
        return $"var result = Result.Failure<{typeParams}>(\"Test error\");";
    }
}
