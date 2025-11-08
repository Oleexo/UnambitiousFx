using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.ValueAccess;

/// <summary>
///     Builds async extension methods for Task and ValueTask Result types.
/// </summary>
internal sealed class AsyncMethodBuilder
{
    private readonly string _baseNamespace;

    public AsyncMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    ///     Builds async ValueOr method with default fallback.
    /// </summary>
    public MethodWriter BuildValueOrDefaultAsync(ushort arity,
                                                 bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        var parameters = new List<MethodParameter> {
            new($"this {asyncResultType}", "awaitableResult")
        };
        parameters.AddRange(Enumerable.Range(1, arity)
                                      .Select(n => new MethodParameter($"TValue{n}", $"fallback{n}")));

        var fallbackArgs = string.Join(", ",
                                       Enumerable.Range(1, arity)
                                                 .Select(n => $"fallback{n}"));

        string body;
        if (arity == 1)
        {
            body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return result.ValueOr(fallback1);
                   """;
        }
        else
        {
            body = $"""
                    var result = await awaitableResult.ConfigureAwait(false);
                    return result.ValueOr({fallbackArgs});
                    """;
        }

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        return new MethodWriter(
            "ValueOrAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            parameters.ToArray(),
            genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async ValueOr returning fallback(s) when failure.")
                                              .Build()
        );
    }

    /// <summary>
    ///     Builds async ValueOr method with factory.
    /// </summary>
    public MethodWriter BuildValueOrFactoryAsync(ushort arity,
                                                 bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");
        var factoryReturn = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        var valueParams = string.Join(", ",
                                      Enumerable.Range(1, arity)
                                                .Select(n => $"value{n}"));

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return result.ValueOr(fallbackFactory);
                   """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        return new MethodWriter(
            "ValueOrAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter($"Func<{factoryReturn}>",  "fallbackFactory")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async ValueOr using fallback factory when failure.")
                                              .Build()
        );
    }

    /// <summary>
    ///     Builds async ValueOrThrow method with default exception.
    /// </summary>
    public MethodWriter BuildValueOrThrowDefaultAsync(ushort arity,
                                                      bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        string body;
        if (arity == 1)
        {
            body = """
                   var result = await resultTask.ConfigureAwait(false);
                   return result.ValueOrThrow();
                   """;
        }
        else
        {
            body = """
                   var result = await resultTask.ConfigureAwait(false);
                   return result.ValueOrThrow(errors => throw errors.ToException());
                   """;
        }

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        return new MethodWriter(
            "ValueOrThrowAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [new MethodParameter($"this {asyncResultType}", "resultTask")],
            genericParams,
            usings: ["System", "System.Threading.Tasks", $"{_baseNamespace}.Results.Reasons"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async ValueOrThrow throwing aggregated exception when failure.")
                                              .Build()
        );
    }

    /// <summary>
    ///     Builds async ValueOrThrow method with custom exception factory.
    /// </summary>
    public MethodWriter BuildValueOrThrowFactoryAsync(ushort arity,
                                                      bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");

        const string body = """
                            var result = await resultTask.ConfigureAwait(false);
                            return result.ValueOrThrow(exceptionFactory);
                            """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        return new MethodWriter(
            "ValueOrThrowAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}",              "resultTask"),
                new MethodParameter("Func<IEnumerable<IError>, Exception>", "exceptionFactory")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks", $"{_baseNamespace}.Results.Reasons"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async ValueOrThrow using exception factory when failure.")
                                              .Build()
        );
    }

    public MethodWriter BuildToNullableDefaultAsync(ushort arity,
                                                    bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";

        string returnType;
        if (arity == 1)
        {
            returnType = "TValue1?";
        }
        else
        {
            var tupleType = GenericTypeHelper.BuildTupleTypeString(arity, "TValue");
            returnType = $"{tupleType}?";
        }

        const string body = """
                            var result = await awaitable.ConfigureAwait(false);
                            return result.ToNullable();
                            """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        return new MethodWriter(
            "ToNullableAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [new MethodParameter($"this {asyncResultType}", "awaitable")],
            genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.ValueAccess"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async ToNullable returning nullable value(s) when success, null when failure.")
                                              .Build()
        );
    }

    /// <summary>
    ///     Builds async Match method for pattern matching on Result.
    /// </summary>
    public MethodWriter BuildMatchAsync(ushort arity,
                                        bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";

        // Build generic parameters: TOut + TValue1, TValue2, ...
        var genericParams = new List<GenericParameter> {
            new("TOut", "notnull")
        };
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));

        // Build success function signature
        var successValueParams = string.Join(", ",
                                             Enumerable.Range(1, arity)
                                                       .Select(n => $"TValue{n}"));
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";
        var successFuncType = $"Func<{successValueParams}, {asyncType}<TOut>>";

        const string body = """
                            var result = await awaitableResult.ConfigureAwait(false);
                            return await result.Match(success, failure);
                            """;

        var asyncReturn = isValueTask
                              ? "ValueTask<TOut>"
                              : "Task<TOut>";

        return new MethodWriter(
            "MatchAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}",                       "awaitableResult"),
                new MethodParameter(successFuncType,                                 "success"),
                new MethodParameter($"Func<IEnumerable<IError>, {asyncType}<TOut>>", "failure")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", $"{_baseNamespace}.Results.Reasons"],
            documentation: DocumentationWriter.Create()
                                              .WithSummary("Async Match for pattern matching on Result, executing success or failure handler.")
                                              .Build()
        );
    }
}
