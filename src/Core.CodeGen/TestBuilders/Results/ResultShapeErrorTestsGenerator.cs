using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ShapeErrorAsync extension methods (Task, ValueTask).
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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) =>
        GenerateVariants(arity, ClassName, (GenerateTaskTests, true), (GenerateValueTaskTests, true));

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultShapeErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ShapeError" };
        cw.AddMethod(new MethodWriter($"ShapeErrorTask_Arity{arity}_Success_ShouldReturnSuccess", "async Task", GenerateTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeErrorTask_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeErrorTaskAwaitable_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateTaskAwaitableFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultShapeErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ShapeError" };
        cw.AddMethod(new MethodWriter($"ShapeErrorValueTask_Arity{arity}_Success_ShouldReturnSuccess", "async Task", GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeErrorValueTask_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.AddMethod(new MethodWriter($"ShapeErrorValueTaskAwaitable_Arity{arity}_Failure_ShouldShapeErrors", "async Task", GenerateValueTaskAwaitableFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings()));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorTaskCall(arity) };
        var then  = new[] { "Assert.True(shaped.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorTaskCall(arity) };
        var then  = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateTaskAwaitableFailureBody(ushort arity) {
        var given = new[] { GenerateTaskFailureResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorTaskAwaitableCall(arity) };
        var then  = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var given = arity == 0 ? new[] { GenerateResultCreation(arity) } : new[] { GenerateTestValues(arity), GenerateResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorValueTaskCall(arity) };
        var then  = new[] { "Assert.True(shaped.IsSuccess);" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var given = new[] { GenerateFailureResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorValueTaskCall(arity) };
        var then  = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateValueTaskAwaitableFailureBody(ushort arity) {
        var given = new[] { GenerateValueTaskFailureResultCreation(arity) };
        var when  = new[] { GenerateShapeErrorValueTaskAwaitableCall(arity) };
        var then  = new[] { "Assert.False(shaped.IsSuccess);", "Assert.Contains(shaped.Errors, e => e.Message.Contains(\"Shaped\"));" };
        return BuildTestBody(given, when, then);
    }

    private string GenerateShapeErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var shaped = await result.ShapeErrorAsync(errors => Task.FromResult(errors.Concat([new Error(\"Shaped\")])));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var shaped = await result.ShapeErrorAsync<{typeParams}>(errors => Task.FromResult(errors.Concat([new Error(\"Shaped\")])));";
    }

    private string GenerateShapeErrorTaskAwaitableCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var shaped = await taskResult.ShapeErrorAsync(errors => Task.FromResult(errors.Concat([new Error(\"Shaped\")])));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var shaped = await taskResult.ShapeErrorAsync<{typeParams}>(errors => Task.FromResult(errors.Concat([new Error(\"Shaped\")])));";
    }

    private string GenerateShapeErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var shaped = await result.ShapeErrorAsync(errors => ValueTask.FromResult(errors.Concat([new Error(\"Shaped\")])));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var shaped = await result.ShapeErrorAsync<{typeParams}>(errors => ValueTask.FromResult(errors.Concat([new Error(\"Shaped\")])));";
    }

    private string GenerateShapeErrorValueTaskAwaitableCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var shaped = await valueTaskResult.ShapeErrorAsync(errors => ValueTask.FromResult(errors.Concat([new Error(\"Shaped\")])));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var shaped = await valueTaskResult.ShapeErrorAsync<{typeParams}>(errors => ValueTask.FromResult(errors.Concat([new Error(\"Shaped\")])));";
    }
}
