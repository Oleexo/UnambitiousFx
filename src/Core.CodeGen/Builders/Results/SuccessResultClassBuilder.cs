using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Results;

internal static class SuccessResultClassBuilder {
    public static ClassWriter Build(ushort arity) {
        var genericParams = ResultArityHelpers.MakeGenericParams(arity);
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var baseClassName = $"Result<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: "SuccessResult",
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName),
            interfaces: [new TypeDefinitionReference("ISuccessResult")]
        );

        // Fields
        for (int i = 1; i <= arity; i++) {
            classWriter.AddField(new FieldWriter(
                                     name: $"_value{i}",
                                     type: $"TValue{i}",
                                     visibility: Visibility.Private,
                                     isReadonly: true
                                 ));
        }

        AddConstructors(arity, classWriter);

        // Flags
        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsFaulted",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));
        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsSuccess",
                                    type: "bool",
                                    visibility: Visibility.Public,
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
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: $"action({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfFailure",
                                  returnType: "void",
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action<IEnumerable<IError>>", "action")]
                              ));

        AddTryGetMethods(arity, classWriter);

        AddDeconstructMethod(arity, classWriter);

        return classWriter;
    }

    private static void AddDeconstructMethod(ushort      arity,
                                             ClassWriter classWriter) {
        var deconstructParams = new List<MethodParameter> { new("out bool", "isSuccess") };
        if (arity == 1) {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else deconstructParams.Add(new MethodParameter($"out ({ResultArityHelpers.JoinValueTypes(arity)})?", "value"));

        deconstructParams.Add(new MethodParameter("out IEnumerable<IError>?", "error"));

        var deconstructBody = arity == 1
                                  ? "isSuccess = true;\nvalue = _value1;\nerror = null;"
                                  : $"isSuccess = true;\nvalue = ({string.Join(", ", Enumerable.Range(1, arity).Select(i => $"_value{i}"))});\nerror = null;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "Deconstruct",
                                  returnType: "void",
                                  body: deconstructBody,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: deconstructParams.ToArray(),
                                  usings: ["UnambitiousFx.Core.Results.Reasons"]
                              ));
    }

    private static void AddTryGetMethods(ushort      arity,
                                         ClassWriter classWriter) {
        var tryGetParamsNoErr = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) tryGetParamsNoErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));

        var tryGetBody1 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = _value{i};")) +
                          "\nreturn true;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "TryGet",
                                  returnType: "bool",
                                  body: tryGetBody1,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: tryGetParamsNoErr.ToArray(),
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
                                  name: "TryGet",
                                  returnType: "bool",
                                  body: tryGetBody2,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: tryGetParamsErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "TryGet",
                                  returnType: "bool",
                                  body: """
                                        errors = null;
                                        return true;
                                        """,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "errors")],
                                  usings: ["System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons"]
                              ));
    }

    private static void AddMatchMethods(ClassWriter classWriter,
                                        string      valueArgs,
                                        string      valueParams) {
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: $"success({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Action<{valueParams}>",      "success"),
                                      new MethodParameter("Action<IEnumerable<IError>>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: $"return success({valueArgs});",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Func<{valueParams}, TOut>",      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
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
                                       className: "SuccessResult",
                                       body: ctorBody,
                                       visibility: Visibility.Public,
                                       parameters: ctorParams
                                   ));
    }

    private static void AddBaseMatchMethodsForSuccess(ClassWriter classWriter) {
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "success();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Action",                      "success"),
                                      new MethodParameter("Action<IEnumerable<IError>>", "failure")
                                  ]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "TOut",
                                  body: "return success();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter("Func<TOut>",                      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: "action();",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action", "action")]
                              ));
    }
}
