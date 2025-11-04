using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.ToNullable extension methods (sync, Task, ValueTask).
/// </summary>
internal sealed class ResultToNullableTestsGenerator : ResultTestGeneratorBase {
    private const int    StartArity          = 1;
    private const string ClassName           = "ResultToNullableTests";
    private const string ExtensionsNamespace = "Results.Extensions.ValueAccess";

    public ResultToNullableTestsGenerator(string               baseNamespace,
                                          FileOrganizationMode fileOrganization)
        : base(new GenerationConfig(baseNamespace, StartArity, ExtensionsNamespace, ClassName, fileOrganization, true)) {
    }

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) => GenerateVariants(arity,
                                                                                                           ClassName,
                                                                                                           (GenerateSyncTests, false),
                                                                                                           (GenerateTaskTests, true),
                                                                                                           (GenerateValueTaskTests, true));

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ToNullable" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Task ToNullable" };
        cw.AddMethod(GenerateTaskSuccessTest(arity));
        cw.AddMethod(GenerateTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.Tasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.Tasks";
        return cw;
    }

    private ClassWriter GenerateValueTaskTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableValueTaskTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - ValueTask ToNullable" };
        cw.AddMethod(GenerateValueTaskSuccessTest(arity));
        cw.AddMethod(GenerateValueTaskFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess.ValueTasks");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.ValueTasks";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) => new(
        $"ToNullable_Arity{arity}_Success_ShouldReturnValue",
        "void",
        GenerateSyncSuccessBody(arity),
        attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateSyncFailureTest(ushort arity) => new($"ToNullable_Arity{arity}_Failure_ShouldReturnNull", "void", GenerateSyncFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateTaskSuccessTest(ushort arity) => new($"ToNullableTask_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateTaskSuccessBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateTaskFailureTest(ushort arity) => new($"ToNullableTask_Arity{arity}_Failure_ShouldReturnNull", "async Task", GenerateTaskFailureBody(arity),
                                                                      attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateValueTaskSuccessTest(ushort arity) => new($"ToNullableValueTask_Arity{arity}_Success_ShouldReturnValue", "async Task",
                                                                           GenerateValueTaskSuccessBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private MethodWriter GenerateValueTaskFailureTest(ushort arity) => new($"ToNullableValueTask_Arity{arity}_Failure_ShouldReturnNull", "async Task",
                                                                           GenerateValueTaskFailureBody(arity), attributes: [new FactAttributeReference()], usings: GetUsings());

    private string GenerateSyncSuccessBody(ushort arity) {
        var          testValues = GenerateTestValues(arity);
        var          creation   = GenerateResultCreation(arity);
        const string call       = "var nullableValue = result.ToNullable();";
        var          assertions = GenerateToNullableSuccessAssertions(arity);
        return BuildTestBody([testValues, creation],
                             [call],
                             assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateSyncFailureBody(ushort arity) {
        var failureCreation = GenerateFailureResultCreation(arity);
        var call            = "var nullableValue = result.ToNullable();";
        var assertions      = GenerateToNullableFailureAssertions();
        return BuildTestBody([failureCreation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateTaskResultCreation(arity);
        var call       = "var nullableValue = await taskResult.ToNullableAsync();";
        var assertions = GenerateToNullableSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateTaskFailureBody(ushort arity) {
        var creation   = GenerateTaskFailureResultCreation(arity);
        var call       = "var nullableValue = await taskResult.ToNullableAsync();";
        var assertions = GenerateToNullableFailureAssertions();
        return BuildTestBody([creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskSuccessBody(ushort arity) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateValueTaskResultCreation(arity);
        var call       = "var nullableValue = await valueTaskResult.ToNullableAsync();";
        var assertions = GenerateToNullableSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateValueTaskFailureBody(ushort arity) {
        var creation   = GenerateValueTaskFailureResultCreation(arity);
        var call       = "var nullableValue = await valueTaskResult.ToNullableAsync();";
        var assertions = GenerateToNullableFailureAssertions();
        return BuildTestBody([creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateToNullableSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return "Assert.Equal(value1, nullableValue.GetValueOrDefault());";
        }

        var equals = string.Join('\n', Enumerable.Range(1, arity)
                                                 .Select(i => $"Assert.Equal(value{i}, nullableValue.Value.Item{i});"));
        return $"Assert.NotNull(nullableValue);\n{equals}";
    }

    private string GenerateToNullableFailureAssertions() => "Assert.Equal(default, nullableValue.GetValueOrDefault());"; // arity-independent (default tuple == null)
}
