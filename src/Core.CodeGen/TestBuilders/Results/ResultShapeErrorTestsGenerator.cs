using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ShapeError extension methods (Sync, Task, ValueTask).
/// </summary>
internal sealed class ResultShapeErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultShapeErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultShapeErrorTestsGenerator(string baseNamespace,
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
                                (x => GenerateSyncTests(x), false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultShapeErrorTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ShapeError" };
        cw.AddMethod(new MethodWriter($"ShapeError_Arity{arity}_Success_ShouldReturnSuccess", "void", GenerateSyncSuccessBody(arity),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeError_Arity{arity}_Failure_ShouldShapeErrors", "void", GenerateSyncFailureBody(arity),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ErrorHandling");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType)
    {
        var cw = new ClassWriter($"ResultShapeError{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} ShapeError" };
        cw.AddMethod(new MethodWriter($"ShapeError{asyncType}_Arity{arity}_Success_ShouldReturnSuccess", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeError{asyncType}_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeError{asyncType}Awaitable_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateAsyncAwaitableFailureBody(arity, asyncType),
                                      attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ErrorHandling.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType)
    {
        var given = arity == 0
                        ? new[] { GenerateResultCreation(arity) }
                        : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateShapeErrorAsyncCall(arity, asyncType) };
        var then = new[] { "Assert.True(shaped.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType)
    {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when = new[] { GenerateShapeErrorAsyncCall(arity, asyncType) };
        var then = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncAwaitableFailureBody(ushort arity,
                                                     string asyncType)
    {
        var given = new[] { GenerateAsyncFailureResultCreation(arity, asyncType) };
        var when = new[] { GenerateShapeErrorAwaitableAsyncCall(arity, asyncType) };
        var then = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateAsyncFailureResultCreation(ushort arity,
                                                      string asyncType)
    {
        var varName = $"{asyncType.ToLowerInvariant()}Result";
        if (arity == 0)
        {
            return $"var {varName} = {asyncType}.FromResult(Result.Failure(\"Test error\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var {varName} = {asyncType}.FromResult(Result.Failure<{typeParams}>(\"Test error\"));";
    }

    private string GenerateShapeErrorAsyncCall(ushort arity,
                                               string asyncType)
    {
        var typeParams = arity == 0
                             ? ""
                             : $"<{GenerateTypeParams(arity)}>";
        return
            $"var shaped = await result.ShapeErrorAsync{typeParams}(errors => {asyncType}.FromResult<IEnumerable<IError>>(errors.Select(e => new Error(e.Message + \" Shaped\"))));";
    }

    private string GenerateShapeErrorAwaitableAsyncCall(ushort arity,
                                                        string asyncType)
    {
        var varName = $"{asyncType.ToLowerInvariant()}Result";
        var typeParams = arity == 0
                             ? ""
                             : $"<{GenerateTypeParams(arity)}>";
        return
            $"var shaped = await {varName}.ShapeErrorAsync{typeParams}(errors => {asyncType}.FromResult<IEnumerable<IError>>(errors.Select(e => new Error(e.Message + \" Shaped\"))));";
    }

    // Synchronous test generation methods
    private string GenerateSyncSuccessBody(ushort arity)
    {
        var given = arity == 0
                        ? new[] { GenerateResultCreation(arity) }
                        : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when = new[] { GenerateShapeErrorSyncCall(arity) };
        var then = new[] { "Assert.True(shaped.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when = new[] { GenerateShapeErrorSyncCall(arity) };
        var then = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateShapeErrorSyncCall(ushort arity)
    {
        var typeParams = arity == 0
                             ? ""
                             : $"<{GenerateTypeParams(arity)}>";
        return $"var shaped = result.ShapeError{typeParams}(errors => errors.Select(e => new Error(e.Message + \" Shaped\")));";
    }
}
