using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

internal sealed class ResultBindTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultBindTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultBindTestsGenerator(string               baseNamespace,
                                    FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (x => GenerateAsyncTests(x, "Task"), true), (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultBindSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Bind" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Transformations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter($"Bind_Arity{arity}_Success_ShouldTransform", "void", GenerateSyncSuccessBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"Bind_Arity{arity}_Failure_ShouldNotTransform", "void", GenerateSyncFailureBody(arity), attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateResultCreation(arity);
        var call       = GenerateBindSyncCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation = GenerateFailureResultCreation(arity);
        var call     = GenerateBindSyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = GenerateBindTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call     = GenerateBindTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = GenerateBindValueTaskCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call     = GenerateBindValueTaskCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateBindSyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = result.Bind(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = result.Bind(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await taskResult.BindAsync(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await taskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindValueTaskCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await valueTaskResult.BindAsync(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await valueTaskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }

    private string GenerateBindSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return "Assert.True(transformedResult.IsSuccess);"; // keep simple
        }

        return "Assert.True(transformedResult.IsSuccess);";
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultBind{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Bind" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Transformations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Bind{asyncType}_Arity{arity}_Success_ShouldTransform", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"Bind{asyncType}_Arity{arity}_Failure_ShouldNotTransform", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateAsyncResultCreation(arity, asyncType);
        var call       = GenerateBindAsyncCall(arity);
        var assertions = GenerateBindSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call     = GenerateBindAsyncCall(arity);
        return BuildTestBody([creation], [call], ["Assert.False(transformedResult.IsSuccess);"]);
    }

    private string GenerateAsyncResultCreation(ushort arity,
                                               string asyncType) {
        string core;
        if (arity == 1) {
            core = "Result.Success(value1)";
        }
        else {
            var values = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"value{i}"));
            core = $"Result.Success({values})";
        }

        return $"var taskResult = {asyncType}.FromResult({core});";
    }

    private string GenerateAsyncFailureResultCreation(ushort arity,
                                                      string asyncType) {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(GetTestType));
        var core = $"Result.Failure<{typeParams}>(\"Test error\")";
        return $"var taskResult = {asyncType}.FromResult({core});";
    }

    private string GenerateBindAsyncCall(ushort arity) {
        if (arity == 1) {
            return "var transformedResult = await taskResult.BindAsync(x => Result.Success(x * 2));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(i => $"x{i}"));
        var mapped = string.Join(", ", Enumerable.Range(1, arity)
                                                 .Select(i => $"x{i} + \"_bound\""));
        return $"var transformedResult = await taskResult.BindAsync(({parameters}) => Result.Success({mapped}));";
    }
}
