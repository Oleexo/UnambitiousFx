using System.CodeDom.Compiler;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen;

internal sealed class OneOfTestsGenerator : ICodeGenerator {
    private readonly string _baseNamespace;
    private const    int    StartArity    = 2;
    private const    string ClassName     = "OneOf";
    private const    string DirectoryName = ClassName;

    public OneOfTestsGenerator(string baseNamespace) {
        _baseNamespace = baseNamespace;
    }

    public void Generate(ushort numberOfArity,
                         string outputPath) {
        for (ushort i = StartArity; i <= numberOfArity; i++) {
            GenerateTestClass(i, outputPath);
        }
    }

    private void GenerateTestClass(ushort arity,
                                   string outputPath) {
        var fileWriter = new FileWriter($"{_baseNamespace}.Tests.{ClassName}");

        var classWriter = CreateTestClass(arity);
        fileWriter.AddClass(classWriter);
        fileWriter.AddUsing("UnambitiousFx.Core.OneOf");

        outputPath = Path.Combine(outputPath, DirectoryName);
        if (!Directory.Exists(outputPath)) {
            Directory.CreateDirectory(outputPath);
        }
        var fileName = Path.Combine(outputPath, $"{ClassName}{arity}Tests.cs");

        using var stringWriter   = new StringWriter();
        using var indentedWriter = new IndentedTextWriter(stringWriter, Constant.Spacing);
        fileWriter.Write(indentedWriter);

        File.WriteAllText(fileName, stringWriter.ToString());
    }

    private ClassWriter CreateTestClass(ushort arity) {
        var genericParams = Enumerable.Range(1, arity)
                                      .Select(GetTestType)
                                      .ToArray();

        var allTypeParams = string.Join(", ", genericParams);

        var classWriter = new ClassWriter(
            name: $"{ClassName}{arity}Tests",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Sealed
        );

        // Generate From{Position}_ShouldSetIs{Position} tests
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
            var testBody    = GenerateFromPositionTestBody(arity, i);

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_ShouldSetIs{ordinalName}",
                                      returnType: "void",
                                      body: testBody,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")]
                                  ));
        }

        // Generate From{Position}_ShouldStoreValue tests
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
            var testType    = GetTestType(i);
            var testValue   = GetTestValue(i);
            var testBody    = GenerateStoreValueTestBody(arity, i, testValue, ordinalName);

            if (IsValueTestable(i)) {
                classWriter.AddMethod(new MethodWriter(
                                          name: $"From{ordinalName}_ShouldStoreValue",
                                          returnType: "void",
                                          body: testBody,
                                          visibility: Visibility.Public,
                                          attributes: GenerateInlineData(i)
                                             .Concat([new AttributeReference("Theory")]),
                                          parameters: [new MethodParameter(testType, "value")]
                                      ));
            }
            else {
                classWriter.AddMethod(new MethodWriter(
                                          name: $"From{ordinalName}_ShouldStoreValue",
                                          returnType: "void",
                                          body: testBody.Replace("value", testValue),
                                          visibility: Visibility.Public,
                                          attributes: [new AttributeReference("Fact")]
                                      ));
            }
        }

        // Generate Match tests (with response)
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
            var testValue   = GetTestValue(i);
            var testBody    = GenerateMatchWithResponseTestBody(arity, i, testValue, ordinalName);

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_WhenMatchWithResponse_ShouldReturn{ordinalName}Value",
                                      returnType: "void",
                                      body: testBody,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")]
                                  ));
        }

        // Generate Match tests (void)
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
            var testValue   = GetTestValue(i);
            var testBody    = GenerateMatchVoidTestBody(arity, i, testValue, ordinalName);

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_WhenMatch_ShouldCall{ordinalName}Handler",
                                      returnType: "void",
                                      body: testBody,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")]
                                  ));
        }

        // Generate implicit conversion tests
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = GetOrdinalName(i);
            var testValue   = GetTestValue(i);
            var testBody    = GenerateImplicitConversionTestBody(i, allTypeParams, ordinalName, testValue);

            classWriter.AddMethod(new MethodWriter(
                                      name: $"ImplicitConversion_From{ordinalName}_ShouldWork",
                                      returnType: "void",
                                      body: testBody,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")]
                                  ));
        }

        return classWriter;
    }

    private string GenerateFromPositionTestBody(ushort arity,
                                                ushort position) {
        var ordinalName = GetOrdinalName(position);
        var testValue   = GetTestValue(position);
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(GetTestType));

        var body = $"var result = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";

        for (ushort i = 1; i <= arity; i++) {
            var currentOrdinal = GetOrdinalName(i);
            body += i == position
                        ? $"Assert.True(result.Is{currentOrdinal});\n"
                        : $"Assert.False(result.Is{currentOrdinal});\n";
        }

        return body.TrimEnd();
    }

    private string GenerateStoreValueTestBody(ushort arity,
                                              ushort position,
                                              string testValue,
                                              string ordinalName) {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(GetTestType));

        var body = $"var result = OneOf<{genericTypes}>.From{ordinalName}(value);\n\n";
        body += $"Assert.True(result.{ordinalName}(out var extracted));\n";

        if (GetTestType(position) == "bool") {
            body += "Assert.True(extracted);\n";
        }
        else {
            body += "Assert.Equal(value, extracted);\n";
        }

        for (ushort i = 1; i <= arity; i++) {
            if (i != position) {
                var currentOrdinal = GetOrdinalName(i);
                body += $"Assert.False(result.{currentOrdinal}(out _));\n";
            }
        }

        return body.TrimEnd();
    }

    private string GenerateMatchWithResponseTestBody(ushort arity,
                                                     ushort position,
                                                     string testValue,
                                                     string ordinalName) {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(GetTestType));

        var body = $"var oneOf = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";

        // Build the Match call
        body += "var result = oneOf.Match(";

        for (ushort i = 1; i <= arity; i++) {
            if (i > 1) body += ", ";

            if (i == position) {
                body += "x => x";
            }
            else {
                var currentOrdinal = GetOrdinalName(i);
                body += $"_ => {{ Assert.Fail(\"{currentOrdinal} handler was called for OneOf holding {ordinalName} value\"); return default; }}";
            }
        }

        body += ");\n\n";
        if (GetTestType(position) == "bool") {
            body += "Assert.True(result);";
        }
        else {
            body += $"Assert.Equal({testValue}, result);";
        }

        return body;
    }

    private string GenerateMatchVoidTestBody(ushort arity,
                                             ushort position,
                                             string testValue,
                                             string ordinalName) {
        var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(GetTestType));

        var body = $"var oneOf = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";

        // Build the Match call
        body += "oneOf.Match(";

        for (ushort i = 1; i <= arity; i++) {
            if (i > 1) body += ", ";

            if (i == position) {
                if (GetTestType(position) == "bool") {
                    body += "x => { Assert.True(x); }";
                }
                else {
                    body += $"x => {{ Assert.Equal({testValue}, x); }}";
                }
            }
            else {
                var currentOrdinal = GetOrdinalName(i);
                body += $"_ => {{ Assert.Fail(\"{currentOrdinal} handler was called for OneOf holding {ordinalName} value\"); }}";
            }
        }

        body += ");";

        return body;
    }

    private string GenerateImplicitConversionTestBody(ushort position,
                                                      string allTypeParams,
                                                      string ordinalName,
                                                      string testValue) {
        var body = $"OneOf<{allTypeParams}> result = {testValue};\n\n" +
                   $"Assert.True(result.Is{ordinalName});\n" +
                   $"Assert.True(result.{ordinalName}(out var value));\n";

        if (GetTestType(position) == "bool") {
            body += "Assert.True(value);";
        }
        else {
            body += $"Assert.Equal({testValue}, value);";
        }

        return body;
    }

    private string GetOrdinalName(int position) {
        return position switch {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }

    private string GetTestType(int position) {
        return position switch {
            1 => "int",
            2 => "string",
            3 => "bool",
            4 => "double",
            5 => "decimal",
            6 => "long",
            7 => "char",
            8 => "float",
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }

    private string GetTestValue(int position) {
        return position switch {
            1 => "42",
            2 => "\"hello\"",
            3 => "true",
            4 => "3.14",
            5 => "99.99m",
            6 => "1000L",
            7 => "'x'",
            8 => "2.5f",
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }

    private bool IsValueTestable(int position) {
        return position switch {
            1 => true, // int - can use Theory with InlineData
            2 => true, // string - can use Theory with InlineData
            _ => false // other types - use Fact with single value
        };
    }

    private AttributeReference[] GenerateInlineData(int position) {
        return position switch {
            1 => [new AttributeReference("InlineData(0)"), new AttributeReference("InlineData(42)"), new AttributeReference("InlineData(-1)")],
            2 => [new AttributeReference("InlineData(\"\")"), new AttributeReference("InlineData(\"hello\")"), new AttributeReference("InlineData(\"test string\")")],
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }
}
