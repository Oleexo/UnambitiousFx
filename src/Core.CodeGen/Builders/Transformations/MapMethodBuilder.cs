using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
///     Builds Map extension methods for Result types.
/// </summary>
internal sealed class MapMethodBuilder
{
    private readonly string _baseNamespace;

    public MapMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    ///     Builds a standalone Map method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var returnType = $"Result<{outTypes}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        string bindCall;

        if (arity == 1)
        {
            funcSignature = "Func<TValue1, TOut1>";
            bindCall = "value => Result.Success(map(value))";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {outTuple}>";

            var lambdaParams = string.Join(",\n                            ",
                                           Enumerable.Range(1, arity)
                                                     .Select(n => $"value{n}"));
            var mapParams = string.Join(", ",
                                        Enumerable.Range(1, arity)
                                                  .Select(n => $"value{n}"));
            var successParams = string.Join(", ",
                                            Enumerable.Range(1, arity)
                                                      .Select(n => $"items.Item{n}"));

            bindCall = $$$"""
                          ({{{lambdaParams}}}) => {
                                                  var items = map({{{mapParams}}});
                                                  return Result.Success({{{successParams}}});
                                              }
                          """;
        }

        var body = $"""
                    return result.Bind({bindCall});
                    """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Transforms the success value(s) using the provided mapping function.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("map", "The mapping function.")
                                            .WithReturns("A new result with transformed value(s).");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "Map",
            returnType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature,        "map")
            ],
            genericParams.ToArray(),
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Map method: Result + async func.
    /// </summary>
    public MethodWriter BuildAsyncFuncMethod(ushort arity,
                                             bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var returnType = $"Result<{outTypes}>";
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build async function signature
        string funcSignature;
        string body;

        if (arity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<TOut1>>";
            body = """
                   return result.BindAsync(async value =>
                   {
                       var newValue = await map(value).ConfigureAwait(false);
                       return Result.Success(newValue);
                   });
                   """.Replace("{asyncType}", asyncType);
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {asyncType}<{outTuple}>>";

            var matchParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"v{n}"));
            var successParams = string.Join(", ", Enumerable.Range(1, arity)
                                                            .Select(n => $"mapped.Item{n}"));

            body = $$"""
                     return result.BindAsync(
                         async ({{matchParams}}) => {
                             var mapped = await map({{matchParams}}).ConfigureAwait(false);
                             return Result.Success({{successParams}});
                         }
                     );
                     """;
        }

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Map transforming success value(s) using an async mapping function.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("map", "The async mapping function.")
                                            .WithReturns("A task with the transformed result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "MapAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature,        "map")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Map method: Task + sync func.
    /// </summary>
    public MethodWriter BuildTaskSyncFuncMethod(ushort arity,
                                                bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = $"Result<{outTypes}>";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build function signature
        string funcSignature;
        if (arity == 1)
        {
            funcSignature = "Func<TValue1, TOut1>";
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {outTuple}>";
        }

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return result.Map(map);
                   """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Map awaiting result then transforming using a sync mapping function.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("map", "The mapping function.")
                                            .WithReturns("A task with the transformed result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "MapAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature,             "map")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Map method: Task + async func.
    /// </summary>
    public MethodWriter BuildTaskAsyncFuncMethod(ushort arity,
                                                 bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var outTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TOut");
        var resultType = $"Result<{valueTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var returnType = $"Result<{outTypes}>";
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";

        // Build generic parameters
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull"));
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "TOut", "notnull"));

        // Build async function signature
        string funcSignature;
        string body;

        if (arity == 1)
        {
            funcSignature = $"Func<TValue1, {asyncType}<TOut1>>";
            body = """
                   return awaitableResult.BindAsync(async value =>
                   {
                       var newValue = await map(value).ConfigureAwait(false);
                       return Result.Success(newValue);
                   });
                   """.Replace("{asyncType}", asyncType);
        }
        else
        {
            var valueParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"TValue{n}"));
            var outTuple = GenericTypeHelper.BuildTupleTypeString(arity, "TOut");
            funcSignature = $"Func<{valueParams}, {asyncType}<{outTuple}>>";

            var matchParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"v{n}"));
            var successParams = string.Join(", ", Enumerable.Range(1, arity)
                                                            .Select(n => $"mapped.Item{n}"));

            body = $$"""
                     return awaitableResult.BindAsync(
                         async ({{matchParams}}) => {
                             var mapped = await map({{matchParams}}).ConfigureAwait(false);
                             return Result.Success({{successParams}});
                         }
                     );
                     """;
        }

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Map awaiting result then transforming using an async mapping function.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("map", "The async mapping function.")
                                            .WithReturns("A task with the transformed result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Input value type {i}.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TOut{i}", $"Output value type {i}.");
        }

        return new MethodWriter(
            "MapAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature,             "map")
            ],
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }
}
