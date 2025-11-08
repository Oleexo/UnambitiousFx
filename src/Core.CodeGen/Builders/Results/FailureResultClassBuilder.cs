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
            "FailureResult",
            Visibility.Internal,
            ClassModifier.Sealed,
            genericParams,
            new TypeDefinitionReference(baseClassName),
            [new TypeDefinitionReference("IFailureResult")]
        );

        AddConstructors(classWriter);

        classWriter.AddProperty(new PropertyWriter(
                                    "IsFaulted",
                                    "bool",
                                    getterBody: "true",
                                    style: PropertyStyle.Override
                                ));
        classWriter.AddProperty(new PropertyWriter(
                                    "IsSuccess",
                                    "bool",
                                    getterBody: "false",
                                    style: PropertyStyle.Override
                                ));

        AddBaseMatchMethodsForFailure(classWriter);

        var valueParams = ResultArityHelpers.JoinValueTypes(arity);

        AddMatchMethods(classWriter, valueParams);

        classWriter.AddMethod(new MethodWriter(
                                  "IfSuccess",
                                  "void",
                                  "",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter($"Action<{valueParams}>", "action")]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "IfFailure",
                                  "void",
                                  "action(Errors);",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("Action<IEnumerable<IError>>", "action")]
                              ));

        AddTryGetMethods(arity, classWriter);

        AddDeconstructMethod(arity, classWriter);

        return classWriter;
    }

    private static void AddDeconstructMethod(ushort arity,
                                             ClassWriter classWriter)
    {
        var deconstructParams = new List<MethodParameter>();
        if (arity == 1)
        {
            deconstructParams.Add(new MethodParameter("out TValue1?", "value"));
        }
        else
        {
            deconstructParams.AddRange(Enumerable.Range(1, arity)
                                                 .Select(x => new MethodParameter($"out TValue{x}?", $"value{x}")));
        }

        deconstructParams.Add(new MethodParameter("out IEnumerable<IError>?", "error"));

        var deconstructBody = arity == 1
                                  ? "value = default;"
                                  : string.Join('\n', Enumerable.Range(1, arity)
                                                                .Select(i => $"value{i} = default;"));

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

    private static void AddTryGetMethods(ushort arity,
                                         ClassWriter classWriter)
    {
        var tryGetParamsWithErr = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            tryGetParamsWithErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        tryGetParamsWithErr.Add(new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "error"));

        var tryGetBody1 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = default;")) +
                          "\nerror = Errors;\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  tryGetBody1,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  tryGetParamsWithErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons"]
                              ));

        var tryGetParamsNoErr = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            tryGetParamsNoErr.Add(new MethodParameter($"[NotNullWhen(true)] out TValue{i}?", $"value{i}"));
        }

        var tryGetBody2 = string.Join("\n", Enumerable.Range(1, arity)
                                                      .Select(i => $"value{i} = default;")) +
                          "\nreturn false;";

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  tryGetBody2,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  tryGetParamsNoErr.ToArray(),
                                  usings: ["System.Diagnostics.CodeAnalysis"]
                              ));

        classWriter.AddMethod(new MethodWriter(
                                  "TryGet",
                                  "bool",
                                  """
                                  errors = Errors;
                                  return false;   
                                  """,
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("[NotNullWhen(false)] out IEnumerable<IError>?", "errors")],
                                  usings: ["System.Diagnostics.CodeAnalysis", "UnambitiousFx.Core.Results.Reasons"]
                              ));
    }

    private static void AddMatchMethods(ClassWriter classWriter,
                                        string valueParams)
    {
        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "void",
                                  "failure(Errors);",
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
                                  "return failure(Errors);",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [
                                      new MethodParameter($"Func<{valueParams}, TOut>",      "success"),
                                      new MethodParameter("Func<IEnumerable<IError>, TOut>", "failure")
                                  ],
                                  [new GenericParameter("TOut", "")]
                              ));
    }

    private static void AddConstructors(ClassWriter classWriter)
    {
        classWriter.AddConstructor(new ConstructorWriter(
                                       "FailureResult",
                                       """
                                       if (attachPrimaryExceptionalReason) 
                                       {    
                                         AddReason(new ExceptionalError(error));
                                       }
                                       """,
                                       Visibility.Public,
                                       [
                                           new MethodParameter("Exception", "error"),
                                           new MethodParameter("bool",      "attachPrimaryExceptionalReason")
                                       ],
                                       usings: ["UnambitiousFx.Core.Results.Reasons"]
                                   ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       "FailureResult",
                                       "AddReasons(errors);",
                                       Visibility.Public,
                                       [
                                           new MethodParameter("IEnumerable<IError>", "errors")
                                       ],
                                       usings: ["UnambitiousFx.Core.Results.Reasons"]
                                   ));

        classWriter.AddConstructor(new ConstructorWriter(
                                       "FailureResult",
                                       "",
                                       Visibility.Public,
                                       [new MethodParameter("Exception", "error")],
                                       baseCall: "this(error, true)"
                                   ));
    }

    private static void AddBaseMatchMethodsForFailure(ClassWriter classWriter)
    {
        classWriter.AddMethod(new MethodWriter(
                                  "Match",
                                  "void",
                                  "failure(Errors);",
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
                                  "return failure(Errors);",
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
                                  "",
                                  Visibility.Public,
                                  MethodModifier.Override,
                                  [new MethodParameter("Action", "action")]
                              ));
    }
}
