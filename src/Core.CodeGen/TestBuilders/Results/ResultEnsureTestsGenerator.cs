using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Ensure validation extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultEnsureTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 1;
    private const string ClassName = "ResultEnsureTests";
    private const string ExtensionsNamespace = "Results.Extensions.Validations";

    public ResultEnsureTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultEnsureSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Ensure" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddMethod(GenerateSyncValidationFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.Validations");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultEnsure{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} Ensure" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncValidationFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.Validations.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"Ensure_Arity{arity}_ValidCondition_ShouldSucceed",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"Ensure_Arity{arity}_FailureResult_ShouldNotValidate",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncValidationFailureTest(ushort arity)
    {
        return new MethodWriter($"Ensure_Arity{arity}_InvalidCondition_ShouldFail",
                                "void",
                                GenerateSyncValidationFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"Ensure{asyncType}_Arity{arity}_ValidCondition_ShouldSucceed",
                                "async Task",
                                GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType)
    {
        return new MethodWriter($"Ensure{asyncType}_Arity{arity}_FailureResult_ShouldNotValidate",
                                "async Task",
                                GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateAsyncValidationFailureTest(ushort arity,
                                                            string asyncType)
    {
        return new MethodWriter($"Ensure{asyncType}_Arity{arity}_InvalidCondition_ShouldFail",
                                "async Task",
                                GenerateAsyncValidationFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var predicate = GeneratePredicate(arity, true, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call = GenerateEnsureSyncCall();
        var assertions = GenerateEnsureSuccessAssertions();
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var predicate = GeneratePredicate(arity, true, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call = GenerateEnsureSyncCall();
        return BuildTestBody([creation, predicate, errorFactory], [call], ["Assert.False(ensuredResult.IsSuccess);"]);
    }

    private string GenerateSyncValidationFailureBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var predicate = GeneratePredicate(arity, false, null);
        var errorFactory = GenerateErrorFactory(arity, null);
        var call = GenerateEnsureSyncCall();
        var assertions = GenerateValidationFailureAssertions()
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions);
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var predicate = GeneratePredicate(arity, true, asyncType);
        var errorFactory = GenerateErrorFactory(arity, asyncType);
        var call = GenerateEnsureAsyncCall();
        var assertions = GenerateEnsureSuccessAssertions();
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var creation = GenerateAsyncFailureResultCreation(arity, asyncType);
        var predicate = GeneratePredicate(arity, false, asyncType);
        var errorFactory = GenerateErrorFactory(arity, asyncType);
        var call = GenerateEnsureAsyncCall();
        return BuildTestBody([creation, predicate, errorFactory], [call], ["Assert.False(ensuredResult.IsSuccess);"]);
    }

    private string GenerateAsyncValidationFailureBody(ushort arity,
                                                      string asyncType)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var predicate = GeneratePredicate(arity, false, asyncType);
        var errorFactory = GenerateErrorFactory(arity, asyncType);
        var call = GenerateEnsureAsyncCall();
        var assertions = GenerateValidationFailureAssertions()
           .Split('\n', StringSplitOptions.RemoveEmptyEntries);
        return BuildTestBody([testValues, creation, predicate, errorFactory], [call], assertions);
    }

    private string GenerateEnsureAsyncCall()
    {
        return "var ensuredResult = await taskResult.EnsureAsync(predicate, errorFactory);";
    }

    // Helper methods restored after refactor
    private string GeneratePredicate(ushort arity,
                                     bool isValid,
                                     string? asyncType)
    {
        var validCondition = isValid
                                 ? "true"
                                 : "false";
        if (arity == 0)
        {
            return asyncType is not null
                       ? $"Func<{asyncType}<bool>> predicate = () => {asyncType}.FromResult({validCondition});"
                       : $"Func<bool> predicate = () => {validCondition};";
        }


        return asyncType is not null
                   ? $"Func<{GenerateTypeParams(arity)}, {asyncType}<bool>> predicate = ({GenerateValueParams(arity)}) => {asyncType}.FromResult({validCondition});"
                   : $"Func<{GenerateTypeParams(arity)}, bool> predicate = ({GenerateValueParams(arity)}) => {validCondition};";
    }

    private string GenerateErrorFactory(ushort arity,
                                        string? asyncType)
    {
        if (asyncType is not null)
        {
            return
                $"Func<{GenerateTypeParams(arity)}, {asyncType}<IError>> errorFactory = ({GenerateValueParams(arity)}) => {asyncType}.FromResult<IError>(new Error(\"Validation failed\"));";
        }

        return $"Func<{GenerateTypeParams(arity)}, IError> errorFactory = ({GenerateValueParams(arity)}) => new Error(\"Validation failed\");";
    }

    private string GenerateEnsureSyncCall()
    {
        return "var ensuredResult = result.Ensure(predicate, errorFactory);";
    }

    private string GenerateEnsureSuccessAssertions()
    {
        return "Assert.True(ensuredResult.IsSuccess);";
    }

    private string GenerateValidationFailureAssertions()
    {
        return "Assert.False(ensuredResult.IsSuccess);";
    }
}
