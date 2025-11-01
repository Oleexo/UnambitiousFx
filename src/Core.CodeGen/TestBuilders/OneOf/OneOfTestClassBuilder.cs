using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.TestBuilders.OneOf;

/// <summary>
/// Builds OneOf test classes with comprehensive test methods.
/// </summary>
internal static class OneOfTestClassBuilder {
    public static ClassWriter Build(ushort arity) {
        var classWriter = new ClassWriter(
            name: $"OneOf{arity}Tests",
            visibility: Visibility.Public,
            classModifiers: ClassModifier.Sealed
        );

        // Generate different types of tests
        AddFromPositionTests(classWriter, arity);
        AddStoreValueTests(classWriter, arity);
        AddMatchWithResponseTests(classWriter, arity);
        AddMatchVoidTests(classWriter, arity);
        AddImplicitConversionTests(classWriter, arity);

        return classWriter;
    }

    private static void AddFromPositionTests(ClassWriter classWriter,
                                             ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var testValue   = TestTypeHelper.GetTestValue(i);
            var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                           .Select(TestTypeHelper.GetTestType));

            var body = $"var result = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";
            for (ushort j = 1; j <= arity; j++) {
                var currentOrdinal = OrdinalHelper.GetOrdinalName(j);
                body += j == i
                            ? $"Assert.True(result.Is{currentOrdinal});\n"
                            : $"Assert.False(result.Is{currentOrdinal});\n";
            }

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_ShouldSetIs{ordinalName}",
                                      returnType: "void",
                                      body: body.TrimEnd(),
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")],
                                      usings: ["UnambitiousFx.Core.OneOf"]
                                  ));
        }
    }

    private static void AddStoreValueTests(ClassWriter classWriter,
                                           ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var testType    = TestTypeHelper.GetTestType(i);
            var testValue   = TestTypeHelper.GetTestValue(i);
            var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                           .Select(TestTypeHelper.GetTestType));

            var body = $"var result = OneOf<{genericTypes}>.From{ordinalName}(value);\n\n";
            body += $"Assert.True(result.{ordinalName}(out var extracted));\n";
            body += testType == "bool"
                        ? "Assert.True(extracted);\n"
                        : "Assert.Equal(value, extracted);\n";

            for (ushort j = 1; j <= arity; j++) {
                if (j != i) {
                    var currentOrdinal = OrdinalHelper.GetOrdinalName(j);
                    body += $"Assert.False(result.{currentOrdinal}(out _));\n";
                }
            }

            if (TestTypeHelper.IsValueTestable(i)) {
                classWriter.AddMethod(new MethodWriter(
                                          name: $"From{ordinalName}_ShouldStoreValue",
                                          returnType: "void",
                                          body: body.TrimEnd(),
                                          visibility: Visibility.Public,
                                          attributes: GenerateInlineData(i)
                                             .Concat([new AttributeReference("Theory")]),
                                          parameters: [new MethodParameter(testType, "value")],
                                          usings: ["UnambitiousFx.Core.OneOf"]
                                      ));
            }
            else {
                classWriter.AddMethod(new MethodWriter(
                                          name: $"From{ordinalName}_ShouldStoreValue",
                                          returnType: "void",
                                          body: body.Replace("value", testValue)
                                                    .TrimEnd(),
                                          visibility: Visibility.Public,
                                          attributes: [new AttributeReference("Fact")],
                                          usings: ["UnambitiousFx.Core.OneOf"]
                                      ));
            }
        }
    }

    private static void AddMatchWithResponseTests(ClassWriter classWriter,
                                                  ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var testValue   = TestTypeHelper.GetTestValue(i);
            var testType    = TestTypeHelper.GetTestType(i);
            var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                           .Select(TestTypeHelper.GetTestType));

            var body = $"var oneOf = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";
            body += "var result = oneOf.Match(";

            for (ushort j = 1; j <= arity; j++) {
                if (j > 1) body += ", ";
                if (j == i) {
                    body += "x => x";
                }
                else {
                    var currentOrdinal = OrdinalHelper.GetOrdinalName(j);
                    body += $"_ => {{ Assert.Fail(\"{currentOrdinal} handler was called for OneOf holding {ordinalName} value\"); return default; }}";
                }
            }

            body += ");\n\n";
            body += testType == "bool"
                        ? "Assert.True(result);"
                        : $"Assert.Equal({testValue}, result);";

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_WhenMatchWithResponse_ShouldReturn{ordinalName}Value",
                                      returnType: "void",
                                      body: body,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")],
                                      usings: ["UnambitiousFx.Core.OneOf"]
                                  ));
        }
    }

    private static void AddMatchVoidTests(ClassWriter classWriter,
                                          ushort      arity) {
        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var testValue   = TestTypeHelper.GetTestValue(i);
            var testType    = TestTypeHelper.GetTestType(i);
            var genericTypes = string.Join(", ", Enumerable.Range(1, arity)
                                                           .Select(TestTypeHelper.GetTestType));

            var body = $"var oneOf = OneOf<{genericTypes}>.From{ordinalName}({testValue});\n\n";
            body += "oneOf.Match(";

            for (ushort j = 1; j <= arity; j++) {
                if (j > 1) body += ", ";
                if (j == i) {
                    body += testType == "bool"
                                ? "x => { Assert.True(x); }"
                                : $"x => {{ Assert.Equal({testValue}, x); }}";
                }
                else {
                    var currentOrdinal = OrdinalHelper.GetOrdinalName(j);
                    body += $"_ => {{ Assert.Fail(\"{currentOrdinal} handler was called for OneOf holding {ordinalName} value\"); }}";
                }
            }

            body += ");";

            classWriter.AddMethod(new MethodWriter(
                                      name: $"From{ordinalName}_WhenMatch_ShouldCall{ordinalName}Handler",
                                      returnType: "void",
                                      body: body,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")],
                                      usings: ["UnambitiousFx.Core.OneOf"]
                                  ));
        }
    }

    private static void AddImplicitConversionTests(ClassWriter classWriter,
                                                   ushort      arity) {
        var allTypeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                        .Select(TestTypeHelper.GetTestType));

        for (ushort i = 1; i <= arity; i++) {
            var ordinalName = OrdinalHelper.GetOrdinalName(i);
            var testValue   = TestTypeHelper.GetTestValue(i);
            var testType    = TestTypeHelper.GetTestType(i);

            var body = $"OneOf<{allTypeParams}> result = {testValue};\n\n" +
                       $"Assert.True(result.Is{ordinalName});\n"           +
                       $"Assert.True(result.{ordinalName}(out var value));\n";

            body += testType == "bool"
                        ? "Assert.True(value);"
                        : $"Assert.Equal({testValue}, value);";

            classWriter.AddMethod(new MethodWriter(
                                      name: $"ImplicitConversion_From{ordinalName}_ShouldWork",
                                      returnType: "void",
                                      body: body,
                                      visibility: Visibility.Public,
                                      attributes: [new AttributeReference("Fact")],
                                      usings: ["UnambitiousFx.Core.OneOf"]
                                  ));
        }
    }

    private static AttributeReference[] GenerateInlineData(int position) {
        return position switch {
            1 => [
                new AttributeReference("InlineData(0)"),
                new AttributeReference("InlineData(42)"),
                new AttributeReference("InlineData(-1)")
            ],
            2 => [
                new AttributeReference("InlineData(\"\")"),
                new AttributeReference("InlineData(\"hello\")"),
                new AttributeReference("InlineData(\"test string\")")
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(position))
        };
    }
}