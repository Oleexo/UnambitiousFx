using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Results;

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
                             "Success",
                             $"Result<{allTypeParams}>",
                             $"return new SuccessResult<{allTypeParams}>({allValueArgs});",
                             Visibility.Public,
                             MethodModifier.Static,
                             valueParameters,
                             genericParams
                         ));

        // Failure<T...>(Exception error)
        writer.AddMethod(new MethodWriter(
                             "Failure",
                             $"Result<{allTypeParams}>",
                             $"return new FailureResult<{allTypeParams}>(error);",
                             Visibility.Public,
                             MethodModifier.Static,
                             [new MethodParameter("Exception", "error")],
                             genericParams,
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
                             "Failure",
                             $"Result<{allTypeParams}>",
                             failureIErrorBody,
                             Visibility.Public,
                             MethodModifier.Static,
                             [new MethodParameter("IError", "error")],
                             genericParams,
                             usings: ["System", "UnambitiousFx.Core.Results.Reasons"]
                         ));

        // Failure<T...>(string message)
        writer.AddMethod(new MethodWriter(
                             "Failure",
                             $"Result<{allTypeParams}>",
                             $"return new FailureResult<{allTypeParams}>(new Exception(message));",
                             Visibility.Public,
                             MethodModifier.Static,
                             [new MethodParameter("string", "message")],
                             genericParams,
                             usings: ["System"]
                         ));

        // Failure<T...>(IEnumerable<IError> errors)
        writer.AddMethod(new MethodWriter(
                             "Failure",
                             $"Result<{allTypeParams}>",
                             $"return new FailureResult<{allTypeParams}>(errors);",
                             Visibility.Public,
                             MethodModifier.Static,
                             [new MethodParameter("IEnumerable<IError>", "errors")],
                             genericParams,
                             usings: ["System"]
                         ));

        return writer;
    }
}
