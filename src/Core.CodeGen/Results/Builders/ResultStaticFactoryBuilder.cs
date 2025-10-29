using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Results.Builders;

internal static class ResultStaticFactoryBuilder {
    public static ClassWriter Build(ushort arity) {
        var writer = new ClassWriter("Result", Visibility.Public, ClassModifier.Partial);

        var genericParams = Enumerable.Range(1, arity)
                                      .Select(i => new GenericParameter($"TValue{i}", "notnull"))
                                      .ToArray();

        var valueParameters = Enumerable.Range(1, arity)
                                        .Select(i => new MethodParameter($"TValue{i}", $"value{i}"))
                                        .ToArray();

        var allTypeParams = ResultArityHelpers.JoinValueTypes(arity);
        var allValueArgs = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(i => $"value{i}"));

        // Success<T...>(value1, ..., valueN)
        writer.AddMethod(new MethodWriter(
                             name: "Success",
                             returnType: $"Result<{allTypeParams}>",
                             body: $"return new SuccessResult<{allTypeParams}>({allValueArgs});",
                             visibility: Visibility.Public,
                             modifier: MethodModifier.Static,
                             parameters: valueParameters,
                             genericParameters: genericParams
                         ));

        // Failure<T...>(Exception error)
        writer.AddMethod(new MethodWriter(
                             name: "Failure",
                             returnType: $"Result<{allTypeParams}>",
                             body: $"return new FailureResult<{allTypeParams}>(error);",
                             visibility: Visibility.Public,
                             modifier: MethodModifier.Static,
                             parameters: [new MethodParameter("Exception", "error")],
                             genericParameters: genericParams,
                             usings: ["System"]
                         ));

        // Failure<T...>(IError error)
        var failureIErrorBody = $$"""
                                  var r = new FailureResult<{{allTypeParams}}>(error.Exception ?? new Exception(error.Message), false);
                                  r.AddReason(error);
                                  foreach (var kv in error.Metadata) {
                                      r.AddMetadata(kv.Key, kv.Value);
                                  }

                                  return r;
                                  """;

        writer.AddMethod(new MethodWriter(
                             name: "Failure",
                             returnType: $"Result<{allTypeParams}>",
                             body: failureIErrorBody,
                             visibility: Visibility.Public,
                             modifier: MethodModifier.Static,
                             parameters: [new MethodParameter("IError", "error")],
                             genericParameters: genericParams,
                             usings: ["System", "UnambitiousFx.Core.Results.Reasons"]
                         ));

        // Failure<T...>(string message)
        writer.AddMethod(new MethodWriter(
                             name: "Failure",
                             returnType: $"Result<{allTypeParams}>",
                             body: $"return new FailureResult<{allTypeParams}>(new Exception(message));",
                             visibility: Visibility.Public,
                             modifier: MethodModifier.Static,
                             parameters: [new MethodParameter("string", "message")],
                             genericParameters: genericParams,
                             usings: ["System"]
                         ));

        // Failure<T...>(IEnumerable<IError> errors)
        writer.AddMethod(new MethodWriter(
                             name: "Failure",
                             returnType: $"Result<{allTypeParams}>",
                             body: $"return new FailureResult<{allTypeParams}>(errors);",
                             visibility: Visibility.Public,
                             modifier: MethodModifier.Static,
                             parameters: [new MethodParameter("IEnumerable<IError>", "errors")],
                             genericParameters: genericParams,
                             usings: ["System"]
                         ));

        return writer;
    }
}
