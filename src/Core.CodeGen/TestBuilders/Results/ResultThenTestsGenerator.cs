using UnambitiousFx.Core.CodeGen.Configuration;
using UnambitiousFx.Core.CodeGen.Design;
using UnambitiousFx.Core.CodeGen.TestBuilders.AttributeReferences;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.Results;

/// <summary>
///     Test generator for Result.Then extension methods (sync only).
/// </summary>
internal sealed class ResultThenTestsGenerator : BaseCodeGenerator
{
    private const int StartArity = 1;
    private const string ClassName = "ResultThenTests";
    private const string ExtensionsNamespace = "Results.Extensions.Transformations";

    public ResultThenTestsGenerator(string baseNamespace,
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
        // Generate sync tests
        var sync = GenerateSyncTests(arity);
        if (sync != null)
        {
            classes.Add(sync);
        }
        return classes;
    }

    private ClassWriter? GenerateSyncTests(ushort arity)
    {
        var cw = new ClassWriter($"ResultThenSyncTestsArity{arity}", Visibility.Public) { Region = $"Arity {arity} - Sync Then" };
        cw.AddMethod(GenerateSyncSuccessTest(arity));
        cw.AddMethod(GenerateSyncFailureTest(arity));
        cw.Namespace = $"{Config.BaseNamespace}.{ExtensionsNamespace}";
        return cw;
    }

    private MethodWriter GenerateSyncSuccessTest(ushort arity)
    {
        return new MethodWriter($"Then_Arity{arity}_Success_ShouldTransform",
                                "void",
                                GenerateSyncSuccessTestBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private MethodWriter GenerateSyncFailureTest(ushort arity)
    {
        return new MethodWriter($"Then_Arity{arity}_Failure_ShouldReturnOriginal",
                                "void",
                                GenerateSyncFailureTestBody(arity),
                                attributes: [new FactAttributeReference()],
                                usings: GetUsings());
    }

    private string GenerateSyncSuccessTestBody(ushort arity)
    {
        var testValues = GenerateTestValues(arity);
        var transformedValues = GenerateTransformedValues(arity);
        var creation = GenerateResultCreation(arity);
        var call = GenerateThenCallSuccess(arity);
        var assertions = GenerateSuccessAssertions(arity);
        return $"""
                {testValues}
                {transformedValues}
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateSyncFailureTestBody(ushort arity)
    {
        var creation = GenerateFailureResultCreation(arity);
        var call = GenerateThenCallFailure(arity);
        var assertions = GenerateFailureAssertions();
        return $"""
                {creation}
                {call}
                {assertions}
                """;
    }

    private string GenerateThenCallSuccess(ushort arity)
    {
        if (arity == 1)
        {
            return "var actualResult = result.Then(v1 => Result.Success(transformed1));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"v{i}"));
        var transformedArgs = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"transformed{i}"));
        return $"var actualResult = result.Then(({parameters}) => Result.Success({transformedArgs}));";
    }

    private string GenerateThenCallFailure(ushort arity)
    {
        if (arity == 1)
        {
            return "var actualResult = result.Then(v1 => Result.Success(100));";
        }

        var parameters = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"v{i}"));
        var dummyValues = string.Join(", ", Enumerable.Range(1, arity).Select(GetTransformedValue));
        return $"var actualResult = result.Then(({parameters}) => Result.Success({dummyValues}));";
    }

    private string GenerateSuccessAssertions(ushort arity)
    {
        var assertions = new List<string> {
            "Assert.True(actualResult.IsSuccess);"
        };

        if (arity == 1)
        {
            assertions.Add("Assert.True(actualResult.TryGet(out var actual));");
            assertions.Add("Assert.Equal(transformed1, actual);");
        }
        else
        {
            var outParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"out var actual{i}"));
            assertions.Add($"Assert.True(actualResult.TryGet({outParams}));");
            for (var i = 1; i <= arity; i++)
            {
                assertions.Add($"Assert.Equal(transformed{i}, actual{i});");
            }
        }

        return string.Join("\n", assertions);
    }

    private string GenerateFailureAssertions()
    {
        return "Assert.False(actualResult.IsSuccess);";
    }

    private string GenerateResultCreation(ushort arity)
    {
        if (arity == 1)
        {
            return "var result = Result.Success(testValue1);";
        }

        var values = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"testValue{i}"));
        return $"var result = Result.Success({values});";
    }

    private string GenerateFailureResultCreation(ushort arity)
    {
        var tp = string.Join(", ", Enumerable.Range(1, arity)
                                             .Select(GetTestType));
        return $"var result = Result.Failure<{tp}>(\"Test error\");";
    }

    private string GenerateTestValues(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => $"var testValue{i} = {GetTestValue(i)};");
        return string.Join("\n        ", values);
    }

    private string GenerateTransformedValues(ushort arity)
    {
        var values = Enumerable.Range(1, arity)
                               .Select(i => $"var transformed{i} = {GetTransformedValue(i)};");
        return string.Join("\n        ", values);
    }

    private string GetTestValue(int index)
    {
        return index switch
        {
            1 => "42",
            2 => "\"Hello\"",
            3 => "3.14",
            4 => "true",
            5 => "'A'",
            6 => "123L",
            7 => "7.5f",
            8 => "99m",
            _ => $"{index * 10}"
        };
    }

    private string GetTransformedValue(int index)
    {
        return index switch
        {
            1 => "100",
            2 => "\"World\"",
            3 => "6.28",
            4 => "false",
            5 => "'B'",
            6 => "456L",
            7 => "15.0f",
            8 => "198m",
            _ => $"{index * 20}"
        };
    }

    private string GetTestType(int index)
    {
        return index switch
        {
            1 => "int",
            2 => "string",
            3 => "double",
            4 => "bool",
            5 => "char",
            6 => "long",
            7 => "float",
            8 => "decimal",
            _ => "string"
        };
    }

    private string GenerateTypeParameters(ushort arity)
    {
        var types = Enumerable.Range(1, arity).Select(i => i switch
        {
            1 => "int",
            2 => "string",
            3 => "double",
            4 => "bool",
            5 => "char",
            6 => "long",
            7 => "float",
            8 => "decimal",
            _ => $"T{i}"
        });
        return string.Join(", ", types);
    }

    private IEnumerable<string> GetUsings()
    {
        return [
            "System",
            "Xunit",
            "UnambitiousFx.Core",
            "UnambitiousFx.Core.Results",
            "UnambitiousFx.Core.Results.Extensions.Transformations"
        ];
    }
}
