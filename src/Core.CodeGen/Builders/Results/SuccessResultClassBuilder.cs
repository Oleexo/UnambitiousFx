using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Results;

internal static class SuccessResultClassBuilder {
    public static ClassWriter Build(ushort arity) {
        var genericParams = ResultArityHelpers.MakeGenericParams(arity);
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var baseClassName = $"Result<{allTypeParams}>";

        var classWriter = new ClassWriter(
            "SuccessResult",
            Visibility.Internal,
            ClassModifier.Sealed,
            genericParams,
            new TypeDefinitionReference(baseClassName),
            [new TypeDefinitionReference("ISuccessResult")]
        );

        // Fields
        for (var i = 1; i <= arity; i++) {
            classWriter.AddField(new FieldWriter(
                                     $"_value{i}",
                                     $"TValue{i}"
                                 ));
        }

        AddConstructors(arity, classWriter);

        // Flags
        classWriter.AddProperty(new PropertyWriter(
                                    "IsFaulted",
                                    "bool",
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));
        classWriter.AddProperty(new PropertyWriter(
                                    "IsSuccess",
                                    "bool",
                                    getterBody: "true",
                                    style: PropertyStyle.Override
                                ));

        // Base match/ifs (no values)
        AddBaseMatchMethodsForSuccess(classWriter);

        // With values
        var valueParams = ResultArityHelpers.JoinValueTypes(arity);
        var valueArgs = string.Join(", ", Enumerable.Range(1, arity)
                                                    .Select(i => $"_value{i}"));

        AddMatchMethods(classWriter, valueArgs, valueParams);

        classWriter.AddMethod(new MethodWriter(
                                  "IfSuccess",
                                  "void",
                                  $"action({valueArgs});",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "IfFailure",
                                  "void",
                                  "",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("Action<IEnumerable<IError>>", "action")]
                              ));

        AddTryGetMethods(arity, classWriter);

        AddDeconstructMethod(arity, classWriter);

        return classWriter;
    }

    private static void AddDeconstructMethod(ushort      arity,
                                             ClassWriter classWriter) {
        var deconstructParams = new List<MethodParameter>();
        if (arity == 1) {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else {
            deconstructParams.AddRange(Enumerable.Range(1, arity)
                                                 .Select(x => new MethodParameter($"out TValue{x}?", $"value{x}")));
        }

        deconstructParams.Add(new MethodParameter("out IEnumerable<IError>?", "error"));

        var deconstructBody = arity == 1
                                  ? "value = _value1;"
                                  : string.Join("\n", Enumerable.Range(1, arity)
                                                                .Select(i => $"value{i} = _value{i};"));
        deconstructBody += "\nerror = null;";
        classWriter.AddMethod(new MethodWriter(
                                  "Deconstruct",
                                  "void",
                                  deconstructBody,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  deconstructParams.ToArray(),
                                  usings: ["UnambitiousFx.Core.Results.Reasons"]
                              ));
    }

    private static void AddTryGetMethods(ushort      arity,
                                         ClassWriter classWriter) {
        var tryGetParamsNoErr = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++) {
            tryGetParamsNoErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var tryGetBody1 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = _value{i};")) +
                          "\nreturn true;";

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  tryGetBody1,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  tryGetParamsNoErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        var tryGetParamsErr = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++) {
            tryGetParamsErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        tryGetParamsErr.Add(new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "errors"));

        var tryGetBody2 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = _value{i};")) +
                          "\nerrors = null;\nreturn true;";

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  tryGetBody2,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  tryGetParamsErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  """
                                  errors = null;
                                  return true;
                                  """,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "errors")],
                                  usings: ["System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons"]
                              ));
    }

    private static void AddMatchMethods(ClassWriter classWriter,
                                        string      valueArgs,
                                        string      valueParams) {
        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "void",
                                  $"success({valueArgs});",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [
                                      new MethodParameter($"Action<{valueParams}>",      "success"),
                                      new MethodParameter("Action<IEnumerable<IError>>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "TOut",
                                  $"return success({valueArgs});",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [
                                      new MethodParameter($"Func<{valueParams}, TOut>",      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  [new GenericParameter("TOut", "")]
                              ));
    }

    private static void AddConstructors(ushort      arity,
                                        ClassWriter classWriter) {
        var ctorParams = Enumerable.Range(1, arity)
                                   .Select(i => new MethodParameter($"TValue{i}", $"value{i}"))
                                   .ToArray();
        var ctorBody = string.Join("\n", Enumerable.Range(1, arity)
                                                   .Select(i => $"_value{i} = value{i};"));
        classWriter.AddConstructor(new ConstructorWriter(
                                       "SuccessResult",
                                       ctorBody,
                                       Visibility.Public,
                                       ctorParams
                                   ));
    }

    private static void AddBaseMatchMethodsForSuccess(ClassWriter classWriter) {
        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "void",
                                  "success();",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [
                                      new MethodParameter("Action",                      "success"),
                                      new MethodParameter("Action<IEnumerable<IError>>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "TOut",
                                  "return success();",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [
                                      new MethodParameter("Func<TOut>",                      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "IfSuccess",
                                  "void",
                                  "action();",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("Action", "action")]
                              ));
    }
}
