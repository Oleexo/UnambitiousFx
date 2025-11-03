using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.TryPickError extension methods (Task, ValueTask).
/// </summary>
internal sealed class ResultTryPickErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0;
    private const string ClassName = "ResultTryPickErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultTryPickErrorTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultTryPickErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task TryPickError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureFoundTest(arity));
        cw.AddMethod(GenerateTaskFailureNotFoundTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultTryPickErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask TryPickError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureFoundTest(arity));
        cw.AddMethod(GenerateValueTaskFailureNotFoundTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureFoundTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorTask_Arity{arity}_Failure_ShouldReturnFoundError",
                                "async Task",
                                GenerateTaskFailureFoundBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureNotFoundTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorTask_Arity{arity}_Failure_ShouldReturnNotFound",
                                "async Task",
                                GenerateTaskFailureNotFoundBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorValueTask_Arity{arity}_Success_ShouldReturnFalse",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureFoundTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorValueTask_Arity{arity}_Failure_ShouldReturnFoundError",
                                "async Task",
                                GenerateValueTaskFailureFoundBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureNotFoundTest(ushort arity)
    {
        return new MethodWriter($"TryPickErrorValueTask_Arity{arity}_Failure_ShouldReturnNotFound",
                                "async Task",
                                GenerateValueTaskFailureNotFoundBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        var call = GenerateTryPickErrorTaskCall(arity);
        var givenSection = arity == 0 ? $"// Given\n{creation}" : $"// Given\n{testValues}\n{creation}";
        return $"""
                {givenSection}
                // When
                {call}
                // Then
                Assert.False(result.Success);
                Assert.Null(result.Error);
                """;
    }

    private string GenerateTaskFailureFoundBody(ushort arity)
    {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call = GenerateTryPickErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.True(result.Success);
                Assert.NotNull(result.Error);
                Assert.Equal("Test error", result.Error.Message);
                """;
    }

    private string GenerateTaskFailureNotFoundBody(ushort arity)
    {
        var creation = GenerateTaskErrorTypeFailureResultCreation(arity);
        var call = GenerateTryPickErrorTaskCallNotFound(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(result.Success);
                Assert.Null(result.Error);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = arity == 0 ? string.Empty : GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        var call = GenerateTryPickErrorValueTaskCall(arity);
        var givenSection = arity == 0 ? $"// Given\n{creation}" : $"// Given\n{testValues}\n{creation}";
        return $"""
                {givenSection}
                // When
                {call}
                // Then
                Assert.False(result.Success);
                Assert.Null(result.Error);
                """;
    }

    private string GenerateValueTaskFailureFoundBody(ushort arity)
    {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call = GenerateTryPickErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.True(result.Success);
                Assert.NotNull(result.Error);
                Assert.Equal("Test error", result.Error.Message);
                """;
    }

    private string GenerateValueTaskFailureNotFoundBody(ushort arity)
    {
        var creation = GenerateValueTaskErrorTypeFailureResultCreation(arity);
        var call = GenerateTryPickErrorValueTaskCallNotFound(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(result.Success);
                Assert.Null(result.Error);
                """;
    }

    private string GenerateTryPickErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = await taskResult.TryPickErrorAsync(e => Task.FromResult(e is Error));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var result = await taskResult.TryPickErrorAsync<{typeParams}>(e => Task.FromResult(e is Error));";
    }

    private string GenerateTryPickErrorTaskCallNotFound(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = await taskResult.TryPickErrorAsync(e => Task.FromResult(false));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var result = await taskResult.TryPickErrorAsync<{typeParams}>(e => Task.FromResult(false));";
    }

    private string GenerateTryPickErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = await valueTaskResult.TryPickErrorAsync(e => ValueTask.FromResult(e is Error));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var result = await valueTaskResult.TryPickErrorAsync<{typeParams}>(e => ValueTask.FromResult(e is Error));";
    }

    private string GenerateTryPickErrorValueTaskCallNotFound(ushort arity)
    {
        if (arity == 0)
        {
            return "var result = await valueTaskResult.TryPickErrorAsync(e => ValueTask.FromResult(false));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var result = await valueTaskResult.TryPickErrorAsync<{typeParams}>(e => ValueTask.FromResult(false));";
    }
}
