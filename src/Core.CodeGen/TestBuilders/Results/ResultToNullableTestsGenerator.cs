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

    protected override IReadOnlyCollection<ClassWriter> GenerateForArity(ushort arity) {
        return GenerateVariants(arity,
                                ClassName,
                                (GenerateSyncTests, false),
                                (x => GenerateAsyncTests(x, "Task"), true),
                                (x => GenerateAsyncTests(x, "ValueTask"), true));
    }

    private ClassWriter GenerateSyncTests(ushort arity) {
        var cw = new ClassWriter($"ResultToNullableSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync ToNullable" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.AddUsing("UnambitiousFx.Core.Results.Extensions.ValueAccess");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private ClassWriter GenerateAsyncTests(ushort arity,
                                           string asyncType) {
        var cw = new ClassWriter($"ResultToNullable{asyncType}TestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - {asyncType} ToNullable" };
        cw.AddMethod(GenerateAsyncSuccessTest(arity, asyncType));
        cw.AddMethod(GenerateAsyncFailureTest(arity, asyncType));
        cw.AddUsing($"UnambitiousFx.Core.Results.Extensions.ValueAccess.{asyncType}s");
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}.{asyncType}s";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity) {
        return new MethodWriter(
            $"ToNullable_Arity{arity}_Success_ShouldReturnValue",
            "void",
            GenerateSyncSuccessBody(arity),
            attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity) {
        return new MethodWriter($"ToNullable_Arity{arity}_Failure_ShouldReturnNull", "void", GenerateSyncFailureBody(arity),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncSuccessTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"ToNullable{asyncType}_Arity{arity}_Success_ShouldReturnValue", "async Task", GenerateAsyncSuccessBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

    private MethodWriter GenerateAsyncFailureTest(ushort arity,
                                                  string asyncType) {
        return new MethodWriter($"ToNullable{asyncType}_Arity{arity}_Failure_ShouldReturnNull", "async Task", GenerateAsyncFailureBody(arity, asyncType),
                                attributes: [new FactAttributeReference()], usings: GetUsings());
    }

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

    private string GenerateAsyncSuccessBody(ushort arity,
                                            string asyncType) {
        var testValues = GenerateTestValues(arity);
        var creation   = GenerateAsyncSuccessResultCreation(arity, asyncType);
        var call       = $"var nullableValue = await taskResult.ToNullableAsync();";
        var assertions = GenerateToNullableSuccessAssertions(arity);
        return BuildTestBody([testValues, creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateAsyncFailureBody(ushort arity,
                                            string asyncType) {
        var creation   = GenerateAsyncFailureResultCreation(arity, asyncType);
        var call       = $"var nullableValue = await taskResult.ToNullableAsync();";
        var assertions = GenerateToNullableFailureAssertions();
        return BuildTestBody([creation], [call], assertions.Split('\n', StringSplitOptions.RemoveEmptyEntries));
    }

    private string GenerateToNullableSuccessAssertions(ushort arity) {
        if (arity == 1) {
            return "Assert.Equal(value1, nullableValue);";
        }

        var equals = string.Join('\n', Enumerable.Range(1, arity)
                                                 .Select(i => $"Assert.Equal(value{i}, nullableValue.Value.Item{i});"));
        return $"Assert.NotNull(nullableValue);\n{equals}";
    }

    private string GenerateToNullableFailureAssertions() {
        return "Assert.Equal(default, nullableValue);";
        // arity-independent (default tuple == null)
    }
}
