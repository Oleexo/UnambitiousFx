using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.MapErrors extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultMapErrorsTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultMapErrorsTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultMapErrorsTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultMapErrorsSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync MapErrors" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorsTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task MapErrors" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultMapErrorsValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask MapErrors" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrors_Arity{arity}_Success_ShouldNotMapErrors",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrors_Arity{arity}_Failure_ShouldMapErrors",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrorsTask_Arity{arity}_Success_ShouldNotMapErrors",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorsTask_Arity{arity}_Failure_ShouldMapErrors",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"MapErrorsValueTask_Arity{arity}_Success_ShouldNotMapErrors",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"MapErrorsValueTask_Arity{arity}_Failure_ShouldMapErrors",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateMapErrorsSyncCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateMapErrorsSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.All(mappedResult.Errors, error => Assert.Contains("MAPPED", error.Message));
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        var call = GenerateMapErrorsTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call = GenerateMapErrorsTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.All(mappedResult.Errors, error => Assert.Contains("MAPPED", error.Message));
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        var call = GenerateMapErrorsValueTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(mappedResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call = GenerateMapErrorsValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(mappedResult.IsSuccess);
                Assert.Single(mappedResult.Errors);
                """;
    }

    private string GenerateMapErrorsSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = result.MapErrors(errors => new Error(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var mappedResult = result.MapErrors<{typeParams}>(errors => new Error(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\"));";
    }

    private string GenerateMapErrorsTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = await taskResult.MapErrorsAsync(errors => Task.FromResult(new Exception(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\")));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var mappedResult = await taskResult.MapErrorsAsync<{typeParams}>(errors => Task.FromResult(new Exception(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\")));";
    }

    private string GenerateMapErrorsValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var mappedResult = await valueTaskResult.MapErrorsAsync(errors => ValueTask.FromResult(new Exception(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\")));";
        }

        var typeParams = GenerateTypeParams(arity);
        return
            $"var mappedResult = await valueTaskResult.MapErrorsAsync<{typeParams}>(errors => ValueTask.FromResult(new Exception(string.Join(\", \", errors.Select(e => e.Message)) + \" - MAPPED\")));";
    }
}
