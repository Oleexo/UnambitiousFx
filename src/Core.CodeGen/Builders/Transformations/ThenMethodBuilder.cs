using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
///     Builds Then extension methods for Result types.
/// </summary>
internal sealed class ThenMethodBuilder
{
    /// <summary>
    ///     Builds a standalone Then method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var resultType = $"Result<{genericTypes}>";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build function signature
        string funcSignature;
        string tryGetParams;
        string thenParams;

        if (arity == 1)
        {
            funcSignature = $"Func<T1, {resultType}>";
            tryGetParams = "out var value";
            thenParams = "value";
        }
        else
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(n => $"T{n}"));
            funcSignature = $"Func<{typeParams}, {resultType}>";
            tryGetParams = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(n => $"out var value{n}"));
            thenParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(n => $"value{n}"));
        }

        var body = $$"""
                     if (!result.TryGet({{tryGetParams}})) {
                         return result;
                     }

                     var response = then({{thenParams}});
                     if (copyReasonsAndMetadata) {
                         result.CopyReasonsAndMetadata(response);
                     }

                     return response;
                     """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Chains a transformation that returns a Result of the same type.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("then", "The transformation function.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A new result from the then function.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "Then",
            resultType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature,        "then"),
                new MethodParameter("bool",               "copyReasonsAndMetadata = true")
            ],
            genericParams,
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Then method: Result + async func.
    /// </summary>
    public MethodWriter BuildAsyncFuncMethod(ushort arity,
                                             bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var resultType = $"Result<{genericTypes}>";
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build async function signature
        string funcSignature;
        string tryGetParams;
        string thenParams;

        if (arity == 1)
        {
            funcSignature = $"Func<T1, {asyncType}<{resultType}>>";
            tryGetParams = "out var value";
            thenParams = "value";
        }
        else
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(n => $"T{n}"));
            funcSignature = $"Func<{typeParams}, {asyncType}<{resultType}>>";
            tryGetParams = string.Join(", ", Enumerable.Range(1, arity)
                                                       .Select(n => $"out var value{n}"));
            thenParams = string.Join(", ", Enumerable.Range(1, arity)
                                                     .Select(n => $"value{n}"));
        }

        var body = $$"""
                     if (!result.TryGet({{tryGetParams}})) {
                         return result;
                     }

                     var response = await then({{thenParams}}).ConfigureAwait(false);
                     if (copyReasonsAndMetadata) {
                         result.CopyReasonsAndMetadata(response);
                     }

                     return response;
                     """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{resultType}>"
                              : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Then chaining an async transformation that returns a Result of the same type.")
                                            .WithParameter("result", "The result instance.")
                                            .WithParameter("then", "The async transformation function.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the new result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "ThenAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {resultType}", "result"),
                new MethodParameter(funcSignature,        "then"),
                new MethodParameter("bool",               "copyReasonsAndMetadata = true")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Then method: Task + sync func.
    /// </summary>
    public MethodWriter BuildTaskSyncFuncMethod(ushort arity,
                                                bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var resultType = $"Result<{genericTypes}>";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build function signature
        string funcSignature;
        if (arity == 1)
        {
            funcSignature = $"Func<T1, {resultType}>";
        }
        else
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(n => $"T{n}"));
            funcSignature = $"Func<{typeParams}, {resultType}>";
        }

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return result.Then(then, copyReasonsAndMetadata);
                   """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{resultType}>"
                              : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Then awaiting result then chaining a sync transformation.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("then", "The transformation function.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the new result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "ThenAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature,             "then"),
                new MethodParameter("bool",                    "copyReasonsAndMetadata = true")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Then method: Task + async func.
    /// </summary>
    public MethodWriter BuildTaskAsyncFuncMethod(ushort arity,
                                                 bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var resultType = $"Result<{genericTypes}>";
        var asyncType = isValueTask
                            ? "ValueTask"
                            : "Task";
        var asyncResultType = isValueTask
                                  ? $"ValueTask<{resultType}>"
                                  : $"Task<{resultType}>";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build async function signature
        string funcSignature;
        if (arity == 1)
        {
            funcSignature = $"Func<T1, {asyncType}<{resultType}>>";
        }
        else
        {
            var typeParams = string.Join(", ", Enumerable.Range(1, arity)
                                                         .Select(n => $"T{n}"));
            funcSignature = $"Func<{typeParams}, {asyncType}<{resultType}>>";
        }

        var body = """
                   var result = await awaitableResult.ConfigureAwait(false);
                   return await result.ThenAsync(then, copyReasonsAndMetadata).ConfigureAwait(false);
                   """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{resultType}>"
                              : $"Task<{resultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Then awaiting result then chaining an async transformation.")
                                            .WithParameter("awaitableResult", "The awaitable result instance.")
                                            .WithParameter("then", "The async transformation function.")
                                            .WithParameter("copyReasonsAndMetadata", "Whether to copy reasons and metadata from original result.")
                                            .WithReturns("A task with the new result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "ThenAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncResultType}", "awaitableResult"),
                new MethodParameter(funcSignature,             "then"),
                new MethodParameter("bool",                    "copyReasonsAndMetadata = true")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }
}
