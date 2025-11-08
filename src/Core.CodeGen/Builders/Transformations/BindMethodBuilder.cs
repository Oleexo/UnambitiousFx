using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
///     Builds Bind extension methods for Result types.
///     Bind chains operations that return Result types, propagating failures.
/// </summary>
internal sealed class BindMethodBuilder
{
    /// <summary>
    ///     Builds a standalone synchronous Bind method for a specific input and output arity.
    /// </summary>
    /// <param name="inputArity">Number of input value types (0-8)</param>
    /// <param name="outputArity">Number of output value types (0-8)</param>
    public MethodWriter BuildStandaloneMethod(ushort inputArity,
                                              ushort outputArity)
    {
        // Build input and output types
        var inputResultType = inputArity == 0
                                  ? "Result"
                                  : $"Result<{GenericTypeHelper.BuildGenericTypeString(inputArity, "TValue")}>";

        var outputResultType = outputArity == 0
                                   ? "Result"
                                   : $"Result<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(inputArity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(outputArity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        string matchSuccessParams;
        string bindCallParams;

        if (inputArity == 0)
        {
            funcSignature = $"Func<{outputResultType}>";
            matchSuccessParams = "";
            bindCallParams = "";
        }
        else if (inputArity == 1)
        {
            funcSignature = $"Func<TValue1, {outputResultType}>";
            matchSuccessParams = "v";
            bindCallParams = "v";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                          .Select(n => $"TValue{n}"));
            funcSignature = $"Func<{valueParams}, {outputResultType}>";
            matchSuccessParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                             .Select(n => $"v{n}"));
            bindCallParams = matchSuccessParams;
        }

        // Build failure result construction
        var failureResult = outputArity == 0
                                ? "Result.Failure(err)"
                                : $"Result.Failure<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>(err)";

        // Build method body
        var successLambda = inputArity == 0
                                ? """
                                  () => {
                                              var response = bind();
                                              if (copyReasonsAndMetadata) {
                                                  result.CopyReasonsAndMetadata(response);
                                              }

                                              return response;
                                          }
                                  """
                                : inputArity == 1
                                    ? $$"""
                                        {{matchSuccessParams}} => {
                                                    var response = bind({{bindCallParams}});
                                                    if (copyReasonsAndMetadata) {
                                                        result.CopyReasonsAndMetadata(response);
                                                    }

                                                    return response;
                                                }
                                        """
                                    : $$"""
                                        ({{matchSuccessParams}}) => {
                                                    var response = bind({{bindCallParams}});
                                                    if (copyReasonsAndMetadata) {
                                                        result.CopyReasonsAndMetadata(response);
                                                    }

                                                    return response;
                                                }
                                        """;

        var body = $$"""
                     return result.Match({{successLambda}}, err => {
                         var response = {{failureResult}};
                         if (copyReasonsAndMetadata) {
                             result.CopyReasonsAndMetadata(response);
                         }

                         return response;
                     });
                     """;

        // Build documentation
        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Chains a function that returns a Result, propagating failures.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("bind", "The function to execute if the result is successful.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("The result from the bind function, or a failure result.");

        for (var i = 1; i <= inputArity; i++)
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        for (var i = 1; i <= outputArity; i++)
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "Bind",
            outputResultType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {inputResultType}", "result"),
                new MethodParameter(funcSignature,             "bind"),
                new MethodParameter("bool",                    "copyReasonsAndMetadata = true")
            ],
            genericParams.ToArray(),
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Bind method: Result + async func.
    ///     Pattern: Result -> Func[ValueTask[Result]] -> ValueTask[Result]
    /// </summary>
    public MethodWriter BuildResultWithAsyncFuncMethod(ushort inputArity,
                                                       ushort outputArity,
                                                       bool isValueTask)
    {
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";
        var taskCompletedMethod = isValueTask
                                      ? "new ValueTask<{0}>({1})"
                                      : "Task.FromResult({1})";

        // Build input and output types
        var inputResultType = inputArity == 0
                                  ? "Result"
                                  : $"Result<{GenericTypeHelper.BuildGenericTypeString(inputArity, "TValue")}>";

        var outputResultType = outputArity == 0
                                   ? "Result"
                                   : $"Result<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(inputArity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(outputArity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        string matchSuccessParams;
        string bindCallParams;
        string matchReturnType;

        if (inputArity == 0)
        {
            funcSignature = $"Func<{asyncType}<{outputResultType}>>";
            matchSuccessParams = "";
            bindCallParams = "";
            matchReturnType = $"<{asyncType}<{outputResultType}>>";
        }
        else if (inputArity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<{outputResultType}>>";
            matchSuccessParams = "v";
            bindCallParams = "v";
            matchReturnType = $"<{asyncType}<{outputResultType}>>";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                          .Select(n => $"TValue{n}"));
            funcSignature = $"Func<{valueParams}, {asyncType}<{outputResultType}>>";
            matchSuccessParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                             .Select(n => $"v{n}"));
            bindCallParams = matchSuccessParams;
            matchReturnType = $"<{asyncType}<{outputResultType}>>";
        }

        // Build failure result construction
        var failureResult = outputArity == 0
                                ? "Result.Failure(err)"
                                : $"Result.Failure<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>(err)";

        // Build method body based on Task or ValueTask
        string body;
        if (isValueTask)
        {
            var successLambda = inputArity == 0
                                    ? """
                                      async () => {
                                                  var response = await bind();
                                                  if (copyReasonsAndMetadata) {
                                                      result.CopyReasonsAndMetadata(response);
                                                  }

                                                  return response;
                                              }
                                      """
                                    : inputArity == 1
                                        ? $$"""
                                            async {{matchSuccessParams}} => {
                                                       var response = await bind({{bindCallParams}});
                                                       if (copyReasonsAndMetadata) {
                                                           result.CopyReasonsAndMetadata(response);
                                                       }

                                                       return response;
                                                   }
                                            """
                                        : $$"""
                                            async ({{matchSuccessParams}}) => {
                                                       var response = await bind({{bindCallParams}});
                                                       if (copyReasonsAndMetadata) {
                                                           result.CopyReasonsAndMetadata(response);
                                                       }

                                                       return response;
                                                   }
                                            """;

            body = $$"""
                     return result.Match{{matchReturnType}}({{successLambda}}, err => {
                         var response = {{failureResult}};
                         if (copyReasonsAndMetadata) {
                             result.CopyReasonsAndMetadata(response);
                         }

                         return new ValueTask<{{outputResultType}}>(response);
                     });
                     """;
        }
        else
        {
            var successLambda = inputArity == 0
                                    ? """
                                      async () => {
                                                  var response = await bind();
                                                  if (copyReasonsAndMetadata) {
                                                      result.CopyReasonsAndMetadata(response);
                                                  }

                                                  return response;
                                              }
                                      """
                                    : inputArity == 1
                                        ? $$"""
                                            async {{matchSuccessParams}} => {
                                                       var response = await bind({{bindCallParams}});
                                                       if (copyReasonsAndMetadata) {
                                                           result.CopyReasonsAndMetadata(response);
                                                       }

                                                       return response;
                                                   }
                                            """
                                        : $$"""
                                            async ({{matchSuccessParams}}) => {
                                                       var response = await bind({{bindCallParams}});
                                                       if (copyReasonsAndMetadata) {
                                                           result.CopyReasonsAndMetadata(response);
                                                       }

                                                       return response;
                                                   }
                                            """;

            body = $$"""
                     return result.Match{{matchReturnType}}({{successLambda}}, err => {
                         var response = {{failureResult}};
                         if (copyReasonsAndMetadata) {
                             result.CopyReasonsAndMetadata(response);
                         }

                         return Task.FromResult(response);
                     });
                     """;
        }

        var asyncReturn = $"{asyncType}<{outputResultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Bind chaining an async function that returns a Result.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("bind", "The async function to execute if the result is successful.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the result from the bind function.");

        for (var i = 1; i <= inputArity; i++)
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        for (var i = 1; i <= outputArity; i++)
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "BindAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {inputResultType}", "result"),
                new MethodParameter(funcSignature,             "bind"),
                new MethodParameter("bool",                    "copyReasonsAndMetadata = true")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Bind method: Task[Result] + sync func.
    ///     Pattern: Task[Result] -> Func[Result] -> Task[Result]
    /// </summary>
    public MethodWriter BuildAwaitableWithSyncFuncMethod(ushort inputArity,
                                                         ushort outputArity,
                                                         bool isValueTask)
    {
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";

        // Build input and output types
        var inputResultType = inputArity == 0
                                  ? "Result"
                                  : $"Result<{GenericTypeHelper.BuildGenericTypeString(inputArity, "TValue")}>";

        var outputResultType = outputArity == 0
                                   ? "Result"
                                   : $"Result<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>";

        var asyncInputType = $"{asyncType}<{inputResultType}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(inputArity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(outputArity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        string matchSuccessParams;
        string bindCallParams;

        if (inputArity == 0)
        {
            funcSignature = $"Func<{outputResultType}>";
            matchSuccessParams = "";
            bindCallParams = "";
        }
        else if (inputArity == 1)
        {
            funcSignature = $"Func<TValue1, {outputResultType}>";
            matchSuccessParams = "v";
            bindCallParams = "v";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                          .Select(n => $"TValue{n}"));
            funcSignature = $"Func<{valueParams}, {outputResultType}>";
            matchSuccessParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                             .Select(n => $"v{n}"));
            bindCallParams = matchSuccessParams;
        }

        // Build failure result construction
        var failureResult = outputArity == 0
                                ? "Result.Failure(err)"
                                : $"Result.Failure<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>(err)";

        // Build method body
        var successLambda = inputArity == 0
                                ? """
                                  () => {
                                              var response = bind();
                                              if (copyReasonsAndMetadata) {
                                                  result.CopyReasonsAndMetadata(response);
                                              }

                                              return response;
                                          }
                                  """
                                : inputArity == 1
                                    ? $$"""
                                        {{matchSuccessParams}} => {
                                                    var response = bind({{bindCallParams}});
                                                    if (copyReasonsAndMetadata) {
                                                        result.CopyReasonsAndMetadata(response);
                                                    }

                                                    return response;
                                                }
                                        """
                                    : $$"""
                                        ({{matchSuccessParams}}) => {
                                                    var response = bind({{bindCallParams}});
                                                    if (copyReasonsAndMetadata) {
                                                        result.CopyReasonsAndMetadata(response);
                                                    }

                                                    return response;
                                                }
                                        """;

        var body = $$"""
                     var result = await awaitable;
                     return result.Match({{successLambda}}, err => {
                         var response = {{failureResult}};
                         if (copyReasonsAndMetadata) {
                             result.CopyReasonsAndMetadata(response);
                         }

                         return response;
                     });
                     """;

        var asyncReturn = $"{asyncType}<{outputResultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Bind awaiting result then chaining a sync function.")
                                            .WithParameter("awaitable", "The awaitable result instance.")
                                            .WithParameter("bind", "The function to execute if the result is successful.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the result from the bind function.");

        for (var i = 1; i <= inputArity; i++)
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        for (var i = 1; i <= outputArity; i++)
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "BindAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncInputType}", "awaitable"),
                new MethodParameter(funcSignature,            "bind"),
                new MethodParameter("bool",                   "copyReasonsAndMetadata = true")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Bind method: Task[Result] + async func.
    ///     Pattern: Task[Result] -> Func[Task[Result]] -> Task[Result]
    /// </summary>
    public MethodWriter BuildAwaitableWithAsyncFuncMethod(ushort inputArity,
                                                          ushort outputArity,
                                                          bool isValueTask)
    {
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";

        // Build input and output types
        var inputResultType = inputArity == 0
                                  ? "Result"
                                  : $"Result<{GenericTypeHelper.BuildGenericTypeString(inputArity, "TValue")}>";

        var outputResultType = outputArity == 0
                                   ? "Result"
                                   : $"Result<{GenericTypeHelper.BuildGenericTypeString(outputArity, "TOut")}>";

        var asyncInputType = $"{asyncType}<{inputResultType}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(inputArity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(outputArity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;

        if (inputArity == 0)
        {
            funcSignature = $"Func<{asyncType}<{outputResultType}>>";
        }
        else if (inputArity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<{outputResultType}>>";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, inputArity)
                                                          .Select(n => $"TValue{n}"));
            funcSignature = $"Func<{valueParams}, {asyncType}<{outputResultType}>>";
        }

        // Build method body - simply compose the two async operations
        const string body = """
                            var result = await awaitable;
                            return await result.BindAsync(bind, copyReasonsAndMetadata);
                            """;

        var asyncReturn = $"{asyncType}<{outputResultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Bind awaiting result then chaining an async function.")
                                            .WithParameter("awaitable", "The awaitable result instance.")
                                            .WithParameter("bind", "The async function to execute if the result is successful.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the result from the bind function.");

        for (var i = 1; i <= inputArity; i++)
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        for (var i = 1; i <= outputArity; i++)
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "BindAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncInputType}", "awaitable"),
                new MethodParameter(funcSignature,            "bind"),
                new MethodParameter("bool",                   "copyReasonsAndMetadata = true")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }
}
