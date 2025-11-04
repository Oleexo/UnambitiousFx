using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Ensure validation extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultEnsureTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultEnsureTests";
    private const string ExtensionsNamespace = "Results.Extensions.Validations";

    public ResultEnsureTestsGenerator(string               baseNamespace,
                                      FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace,
                                    StartArity,
                                    ExtensionsNamespace,
                                    ClassName,
                                    fileOrganization,
                                    true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateSyncTests, false), (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Ensure" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncValidationFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Validations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task Ensure" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskValidationFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Validations.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultEnsureValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask Ensure" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskValidationFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Validations.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new($"Ensure_Arity{arity}_ValidCondition_ShouldSucceed",
                                                                      "void",
                                                                      GenerateSyncSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"Ensure_Arity{arity}_FailureResult_ShouldNotValidate",
                                                                      "void",
                                                                      GenerateSyncFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateSyncValidationFailureTest(ushort arity) => new($"Ensure_Arity{arity}_InvalidCondition_ShouldFail",
                                                                                "void",
                                                                                GenerateSyncValidationFailureBody(arity),
                                                                                attributes: [new FactAttributeReference()],
                                                                                usings: GetUsings());

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"EnsureTask_Arity{arity}_ValidCondition_ShouldSucceed",
                                                                      "async Task",
                                                                      GenerateTaskSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"EnsureTask_Arity{arity}_FailureResult_ShouldNotValidate",
                                                                      "async Task",
                                                                      GenerateTaskFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()],
                                                                      usings: GetUsings());

    private MethodWriter GenerateTaskValidationFailureTest(ushort arity) => new($"EnsureTask_Arity{arity}_InvalidCondition_ShouldFail",
                                                                                "async Task",
                                                                                GenerateTaskValidationFailureBody(arity),
                                                                                attributes: [new FactAttributeReference()],
                                                                                usings: GetUsings());

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"EnsureValueTask_Arity{arity}_ValidCondition_ShouldSucceed",
                                                                           "async Task",
                                                                           GenerateValueTaskSuccessBody(arity),
                                                                           attributes: [new FactAttributeReference()],
                                                                           usings: GetUsings());

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"EnsureValueTask_Arity{arity}_FailureResult_ShouldNotValidate",
                                                                           "async Task",
                                                                           GenerateValueTaskFailureBody(arity),
                                                                           attributes: [new FactAttributeReference()],
                                                                           usings: GetUsings());

    private MethodWriter GenerateValueTaskValidationFailureTest(ushort arity) => new($"EnsureValueTask_Arity{arity}_InvalidCondition_ShouldFail",
                                                                                     "async Task",
                                                                                     GenerateValueTaskValidationFailureBody(arity),
                                                                                     attributes: [new FactAttributeReference()],
                                                                                     usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateResultCreation(arity);
        var predicate    = GeneratePredicate(arity, true, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call         = GenerateEnsureSyncCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var creation     = GenerateFailureResultCreation(arity);
        var predicate    = GeneratePredicate(arity, true, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call         = GenerateEnsureSyncCall(arity);
        return BuildTestBody([creation, predicate, errorFactory], [call], ["Assert.False(ensuredResult.IsSuccess);"]);
    }

    private string GenerateSyncValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateResultCreation(arity);
        var predicate    = GeneratePredicate(arity, false, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call         = GenerateEnsureSyncCall(arity);
        var assertions = GenerateValidationFailureAssertions()
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions);
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateTaskResultCreation(arity);
        var predicate    = GeneratePredicate(arity, true, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation     = GenerateTaskFailureResultCreation(arity);
        var predicate    = GeneratePredicate(arity, false, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        return BuildTestBody([creation, predicate, errorFactory], [call], ["Assert.False(ensuredResult.IsSuccess);"]);
    }

    private string GenerateTaskValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateTaskResultCreation(arity);
        var predicate    = GeneratePredicate(arity, false, "Task");
        var errorFactory = GenerateErrorFactory(arity, "Task");
        var call         = GenerateEnsureTaskCall(arity);
        var assertions = GenerateValidationFailureAssertions()
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateValueTaskResultCreation(arity);
        var predicate    = GeneratePredicate(arity, true, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        var assertions   = GenerateEnsureSuccessAssertions(arity);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation     = GenerateValueTaskFailureResultCreation(arity);
        var predicate    = GeneratePredicate(arity, false, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        return BuildTestBody([creation, predicate, errorFactory], [call], ["Assert.False(ensuredResult.IsSuccess);"]);
    }

    private string GenerateValueTaskValidationFailureBody(ushort arity) {
        var testValues   = GenerateTestValues(arity);
        var creation     = GenerateValueTaskResultCreation(arity);
        var predicate    = GeneratePredicate(arity, false, "ValueTask");
        var errorFactory = GenerateErrorFactory(arity, "ValueTask");
        var call         = GenerateEnsureValueTaskCall(arity);
        var assertions = GenerateValidationFailureAssertions()
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions);
    }

    // Helper methods restored after refactor
    private string GeneratePredicate(ushort  arity,
                                     bool    isValid,
                                     string? asyncType) {
        var validCondition = isValid
                                 ? "true"
                                 : "false";
        if (arity == 0) {
            return asyncType is not null
                       ? $"Func<{asyncType}<bool>> predicate = () => {asyncType}.FromResult({validCondition});"
                       : $"Func<bool> predicate = () => {validCondition};";
        }


        return asyncType is not null
                   ? $"Func<{GenerateTypeParams(arity)}, {asyncType}<bool>> predicate = ({GenerateValueParams(arity)}) => {asyncType}.FromResult({validCondition});"
                   : $"Func<{GenerateTypeParams(arity)}, bool> predicate = ({GenerateValueParams(arity)}) => {validCondition};";
    }

    private string GenerateErrorFactory(ushort  arity,
                                        string? asyncType) {
        if (asyncType is not null) {
            return $"Func<{GenerateTypeParams(arity)}, {asyncType}<IError>> errorFactory = ({GenerateValueParams(arity)}) => {asyncType}.FromResult<IError>(new Error(\"Validation failed\"));";
        }
        return $"Func<{GenerateTypeParams(arity)}, IError> errorFactory = ({GenerateValueParams(arity)}) => new Error(\"Validation failed\");";
    }

    private string GenerateEnsureSyncCall(ushort          arity) => "var ensuredResult = result.Ensure(predicate, errorFactory);";
    private string GenerateEnsureTaskCall(ushort          arity) => "var ensuredResult = await taskResult.EnsureAsync(predicate, errorFactory);";
    private string GenerateEnsureValueTaskCall(ushort     arity) => "var ensuredResult = await valueTaskResult.EnsureAsync(predicate, errorFactory);";
    private string GenerateEnsureSuccessAssertions(ushort arity) => "Assert.True(ensuredResult.IsSuccess);";
    private string GenerateValidationFailureAssertions() => "Assert.False(ensuredResult.IsSuccess);";
}
