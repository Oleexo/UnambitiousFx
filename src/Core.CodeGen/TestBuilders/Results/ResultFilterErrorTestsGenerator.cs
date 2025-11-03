using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.FilterError extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultFilterErrorTestsGenerator : ResultTestGeneratorBase
{
    private const int StartArity = 0; // Changed to 0 to support non-generic Result
    private const string ClassName = "ResultFilterErrorTests";
    private const string ExtensionsNamespace = "Results.Extensions.ErrorHandling";

    public ResultFilterErrorTestsGenerator(string baseNamespace,
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
        var cw = new ClassWriter($"ResultFilterErrorSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync FilterError" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter? GenerateTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultFilterErrorTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task FilterError" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter? GenerateValueTaskTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultFilterErrorValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask FilterError" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"FilterError_Arity{arity}_Success_ShouldNotFilterError",
                                "void",
                                GenerateSyncSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"FilterError_Arity{arity}_Failure_ShouldFilterError",
                                "void",
                                GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"FilterErrorTask_Arity{arity}_Success_ShouldNotFilterError",
                                "async Task",
                                GenerateTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"FilterErrorTask_Arity{arity}_Failure_ShouldFilterError",
                                "async Task",
                                GenerateTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity)
    {
        return new MethodWriter($"FilterErrorValueTask_Arity{arity}_Success_ShouldNotFilterError",
                                "async Task",
                                GenerateValueTaskSuccessBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateValueTaskFailureTest(ushort arity)
    {
        return new MethodWriter($"FilterErrorValueTask_Arity{arity}_Failure_ShouldFilterError",
                                "async Task",
                                GenerateValueTaskFailureBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateFilterErrorSyncCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(filteredResult.IsSuccess);
                """;
    }

    private string GenerateSyncFailureBody(ushort arity)
    {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call = GenerateFilterErrorSyncCall(arity);
        return $"""
                // Given
                {failureCreation}
                // When
                {call}
                // Then
                Assert.False(filteredResult.IsSuccess);
                Assert.Single(filteredResult.Errors);
                Assert.Equal("Test error", filteredResult.Errors.First().Message);
                """;
    }

    private string GenerateTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateTaskResultCreation(arity);
        var call = GenerateFilterErrorTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(filteredResult.IsSuccess);
                """;
    }

    private string GenerateTaskFailureBody(ushort arity)
    {
        var creation = GenerateTaskFailureResultCreation(arity);
        var call = GenerateFilterErrorTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(filteredResult.IsSuccess);
                Assert.Single(filteredResult.Errors);
                Assert.Equal("Test error", filteredResult.Errors.First().Message);
                """;
    }

    private string GenerateValueTaskSuccessBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var creation = GenerateValueTaskResultCreation(arity);
        var call = GenerateFilterErrorValueTaskCall(arity);
        return $"""
                // Given
                {testValues}
                {creation}
                // When
                {call}
                // Then
                Assert.True(filteredResult.IsSuccess);
                """;
    }

    private string GenerateValueTaskFailureBody(ushort arity)
    {
        var creation = GenerateValueTaskFailureResultCreation(arity);
        var call = GenerateFilterErrorValueTaskCall(arity);
        return $"""
                // Given
                {creation}
                // When
                {call}
                // Then
                Assert.False(filteredResult.IsSuccess);
                Assert.Single(filteredResult.Errors);
                Assert.Equal("Test error", filteredResult.Errors.First().Message);
                """;
    }

    private string GenerateFilterErrorSyncCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = result.FilterError(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = result.FilterError<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }

    private string GenerateFilterErrorTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = await taskResult.FilterErrorAsync(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = await taskResult.FilterErrorAsync<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }

    private string GenerateFilterErrorValueTaskCall(ushort arity)
    {
        if (arity == 0)
        {
            return "var filteredResult = await valueTaskResult.FilterErrorAsync(error => error.Message.Contains(\"Test\"));";
        }

        var typeParams = GenerateTypeParams(arity);
        return $"var filteredResult = await valueTaskResult.FilterErrorAsync<{typeParams}>(error => error.Message.Contains(\"Test\"));";
    }
}
