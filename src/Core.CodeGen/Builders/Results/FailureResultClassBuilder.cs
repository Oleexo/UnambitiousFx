using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Results;

internal static class FailureResultClassBuilder
{
    public static ClassWriter Build(ushort arity)
    {
        var genericParams = ResultArityHelpers.MakeGenericParams(arity);
        var allTypeParams = string.Join(", ", genericParams.Select(g => g.Name));
        var baseClassName = $"Result<{allTypeParams}>";

        var classWriter = new ClassWriter(
            name: "FailureResult",
            visibility: Visibility.Internal,
            classModifiers: ClassModifier.Sealed,
            genericParameters: genericParams,
            baseClass: new TypeDefinitionReference(baseClassName),
            interfaces: [new TypeDefinitionReference("IFailureResult")]
        );

        AddConstructors(classWriter);

        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsFaulted",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "true",
                                    style: PropertyStyle.Override
                                ));
        classWriter.AddProperty(new PropertyWriter(
                                    name: "IsSuccess",
                                    type: "bool",
                                    visibility: Visibility.Public,
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));

        AddBaseMatchMethodsForFailure(classWriter);

        var valueParams = ResultArityHelpers.JoinValueTypes(arity);

        AddMatchMethods(classWriter, valueParams);

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfSuccess",
                                  returnType: "void",
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  name: "IfFailure",
                                  returnType: "void",
                                  body: "action(Errors);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action<IEnumerable<IError>>", "action")]
                              ));

        AddTryGetMethods(arity, classWriter);

        AddDeconstructMethod(arity, classWriter);

        return classWriter;
    }

    private static void AddDeconstructMethod(ushort arity,
                                             ClassWriter classWriter)
    {
        var deconstructParams = new List<MethodParameter> { new("out bool", "isSuccess") };
        if (arity == 1)
        {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else deconstructParams.Add(new MethodParameter($"out ({ResultArityHelpers.JoinValueTypes(arity)})?", "value"));

        deconstructParams.Add(new MethodParameter("out IEnumerable<IError>?", "error"));

        const string deconstructBody = "isSuccess = false;\nvalue = default;\nerror = Errors;";

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

    private static void AddTryGetMethods(ushort arity,
                                         ClassWriter classWriter)
    {
        var tryGetParamsWithErr = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) tryGetParamsWithErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        tryGetParamsWithErr.Add(new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "error"));

        var tryGetBody1 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = default;")) +
                          "\nerror = Errors;\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "TryGet",
                                  returnType: "bool",
                                  body: tryGetBody1,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: tryGetParamsWithErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons"]
                              ));

        var tryGetParamsNoErr = new List<MethodParameter>();
        for (int i = 1; i <= arity; i++) tryGetParamsNoErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));

        var tryGetBody2 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = default;")) +
                          "\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  name: "TryGet",
                                  returnType: "bool",
                                  body: tryGetBody2,
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: tryGetParamsNoErr.ToArray(),
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
                                        string valueParams)
    {
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "failure(Errors);",
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
                                  body: "return failure(Errors);",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [
                                      new MethodParameter($"Func<{valueParams}, TOut>",      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  genericParameters: [new GenericParameter("TOut", "")]
                              ));
    }

    private static void AddConstructors(ClassWriter classWriter)
    {
        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "FailureResult",
                                       body: """
                                             if (attachPrimaryExceptionalReason) 
                                             {    
                                               AddReason(new ExceptionalError(error));
                                             }
                                             """,
                                       visibility: Visibility.Public,
                                       parameters: [
                                           new MethodParameter("Exception", "error"),
                                           new MethodParameter("bool",      "attachPrimaryExceptionalReason")
                                       ],
                                       usings: ["UnambitiousFx.Core.Results.Reasons"]
                                   ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "FailureResult",
                                       body: "AddReasons(errors);",
                                       visibility: Visibility.Public,
                                       parameters: [
                                           new MethodParameter("IEnumerable<IError>", "errors")
                                       ],
                                       usings: ["UnambitiousFx.Core.Results.Reasons"]
                                   ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       className: "FailureResult",
                                       body: "",
                                       visibility: Visibility.Public,
                                       parameters: [new MethodParameter("Exception", "error")],
                                       baseCall: "this(error, true)"
                                   ));
    }

    private static void AddBaseMatchMethodsForFailure(ClassWriter classWriter)
    {
        classWriter.AddMethod(new MethodWriter(
                                  name: "Match",
                                  returnType: "void",
                                  body: "failure(Errors);",
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
                                  body: "return failure(Errors);",
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
                                  body: "",
                                  visibility: Visibility.Public,
                                  modifier: MethodModifier.Override,
                                  parameters: [new MethodParameter("Action", "action")]
                              ));
    }
}
