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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity)
    {
        var classes = new List<ClassWriter>();

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

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultShapeErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ShapeError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddMethod(GenerateTaskAwaitableFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultShapeErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ShapeError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddMethod(GenerateValueTaskAwaitableFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorTask_Arity{arity}_Success_ShouldReturnSuccess",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorTask_Arity{arity}_Failure_ShouldShapeErrors",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskAwaitableFailureTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorTaskAwaitable_Arity{arity}_Failure_ShouldShapeErrors",
                                "async Task",
                                GenerateTaskAwaitableFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorValueTask_Arity{arity}_Success_ShouldReturnSuccess",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorValueTask_Arity{arity}_Failure_ShouldShapeErrors",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskAwaitableFailureTest(ushort arity)
    {
        return new MethodWriter($"ShapeErrorValueTaskAwaitable_Arity{arity}_Failure_ShouldShapeErrors",
                                "async Task",
                                GenerateValueTaskAwaitableFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateShapeErrorTaskCall(arity);
        var givenSection = arity == 0 ? $"// Given\n{creation}" : $"// Given\n{testValues}\n{creation}";
        return $"""
                {givenSection}
                // When
                {call}
                // Then
                Assert.True(shaped.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateShapeErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(shaped.IsSuccess);
                Assert.Contains(shaped.Errors, e => e.Message.Contains("Shaped"));
                """;
    }

    private string GenerateTaskAwaitableFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call = GenerateShapeErrorTaskAwaitableCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(shaped.IsSuccess);
                Assert.Contains(shaped.Errors, e => e.Message.Contains("Shaped"));
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateShapeErrorValueTaskCall(arity);
        var givenSection = arity == 0 ? $"// Given\n{creation}" : $"// Given\n{testValues}\n{creation}";
        return $"""
                {givenSection}
                // When
                {call}
                // Then
                Assert.True(shaped.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateShapeErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(shaped.IsSuccess);
                Assert.Contains(shaped.Errors, e => e.Message.Contains("Shaped"));
                """;
    }

    private string GenerateValueTaskAwaitableFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call = GenerateShapeErrorValueTaskAwaitableCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(shaped.IsSuccess);
                Assert.Contains(shaped.Errors, e => e.Message.Contains("Shaped"));
                """;
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
