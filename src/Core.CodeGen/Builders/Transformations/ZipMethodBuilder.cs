using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
///     Builds Zip extension methods for Result types.
/// </summary>
internal sealed class ZipMethodBuilder
{
    /// <summary>
    ///     Builds a standalone Zip method without projector for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var returnType = $"Result<{genericTypes}>";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build parameters - each result is Result<T{n}>
        var parameters = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            var paramType = i == 1
                                ? $"this Result<T{i}>"
                                : $"Result<T{i}>";
            parameters.Add(new MethodParameter(paramType, $"r{i}"));
        }

        // Build body - check each result and accumulate errors
        var checks = new List<string>();
        for (var i = 1; i <= arity; i++)
        {
            checks.Add($$"""
                         if (!r{{i}}.TryGet(out var v{{i}}, out var e{{i}})) {
                             return Result.Failure<{{genericTypes}}>(e{{i}});
                         }
                         """);
        }

        var successParams = string.Join(", ", Enumerable.Range(1, arity)
                                                        .Select(n => $"v{n}"));
        var body = $$"""
                     {{string.Join("\n\n        ", checks)}}

                     return Result.Success({{successParams}});
                     """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Combines multiple Result instances into a single Result containing all values.")
                                            .WithReturns("A Result containing all values if all are successful, otherwise the first failure.");

        for (var i = 1; i <= arity; i++)
        {
            docBuilder.WithParameter($"r{i}", $"Result {i} to combine.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "Zip",
            returnType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            parameters.ToArray(),
            genericParams,
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds a standalone Zip method with projector for a specific arity.
    /// </summary>
    public MethodWriter BuildProjectorMethod(ushort arity)
    {
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull"));
        genericParams.Add(new GenericParameter("TR", "notnull"));

        // Build parameters
        var parameters = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            var paramType = i == 1
                                ? $"this Result<T{i}>"
                                : $"Result<T{i}>";
            parameters.Add(new MethodParameter(paramType, $"r{i}"));
        }

        var projectorParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"T{n}"));
        parameters.Add(new MethodParameter($"Func<{projectorParams}, TR>", "projector"));

        // Build body
        var checks = new List<string>();
        for (var i = 1; i <= arity; i++)
        {
            checks.Add($$"""
                         if (!r{{i}}.TryGet(out var v{{i}}, out var e{{i}})) {
                             return Result.Failure<TR>(e{{i}});
                         }
                         """);
        }

        var projectorArgs = string.Join(", ", Enumerable.Range(1, arity)
                                                        .Select(n => $"v{n}"));
        var body = $$"""
                     {{string.Join("\n\n        ", checks)}}

                     return Result.Success(projector({{projectorArgs}}));
                     """;

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Combines multiple Result instances and projects their values using a function.")
                                            .WithReturns("A Result containing the projected value if all are successful, otherwise the first failure.");

        for (var i = 1; i <= arity; i++)
        {
            docBuilder.WithParameter($"r{i}", $"Result {i} to combine.");
        }

        docBuilder.WithParameter("projector", "Function to project the combined values.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        docBuilder.WithTypeParameter("TR", "Projected result type.");

        return new MethodWriter(
            "Zip",
            "Result<TR>",
            body,
            Visibility.Public,
            MethodModifier.Static,
            parameters.ToArray(),
            genericParams.ToArray(),
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Zip method without projector for Task/ValueTask Result types.
    /// </summary>
    public MethodWriter BuildAsyncMethod(ushort arity,
                                         bool isValueTask)
    {
        var genericTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var returnType = $"Result<{genericTypes}>";
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull");

        // Build parameters
        var parameters = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            var asyncType = isValueTask
                                ? "ValueTask"
                                : "Task";
            var paramType = i == 1
                                ? $"this {asyncType}<Result<T{i}>>"
                                : $"{asyncType}<Result<T{i}>>";
            parameters.Add(new MethodParameter(paramType, $"r{i}"));
        }

        // Build body - await all results then zip

        var body = $"""
                    {string.Join('\n', Enumerable.Range(1, arity).Select(x => $"var result{x} = await r{x};"))}
                    return result1.Zip({string.Join(",", Enumerable.Range(2, arity - 1).Select(n => $"result{n}"))});
                    """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{returnType}>"
                              : $"Task<{returnType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Zip combining multiple awaitable Result instances.")
                                            .WithReturns("A task with the combined result.");

        for (var i = 1; i <= arity; i++)
        {
            docBuilder.WithParameter($"r{i}", $"Awaitable result {i} to combine.");
        }

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "ZipAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            parameters.ToArray(),
            genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Zip method with projector for Task/ValueTask Result types.
    /// </summary>
    public MethodWriter BuildProjectorAsyncMethod(ushort arity,
                                                  bool isValueTask)
    {
        var inputTypes = GenericTypeHelper.BuildGenericTypeString(arity);
        var genericParams = new List<GenericParameter>();
        genericParams.AddRange(GenericTypeHelper.CreateGenericParameters(arity, "T", "notnull"));
        genericParams.Add(new GenericParameter("TR", "notnull"));

        // Build parameters
        var parameters = new List<MethodParameter>();
        for (var i = 1; i <= arity; i++)
        {
            var asyncType = isValueTask
                                ? "ValueTask"
                                : "Task";
            var paramType = i == 1
                                ? $"this {asyncType}<Result<T{i}>>"
                                : $"{asyncType}<Result<T{i}>>";
            parameters.Add(new MethodParameter(paramType, $"r{i}"));
        }

        var projectorParams = string.Join(", ", Enumerable.Range(1, arity)
                                                          .Select(n => $"T{n}"));
        parameters.Add(new MethodParameter($"Func<{projectorParams}, TR>", "projector"));

        // Build body

        var body = $$"""
                     {{string.Join('\n', Enumerable.Range(1, arity).Select(x => $"var result{x} = await r{x};"))}}
                     return result1.Zip({{string.Join(",", Enumerable.Range(2, arity - 1).Select(n => $"result{n}"))}}, projector);
                     """;

        var asyncReturn = isValueTask
                              ? "ValueTask<Result<TR>>"
                              : "Task<Result<TR>>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Zip combining multiple awaitable Result instances with projection.")
                                            .WithReturns("A task with the projected result.");

        for (var i = 1; i <= arity; i++)
        {
            docBuilder.WithParameter($"r{i}", $"Awaitable result {i} to combine.");
        }

        docBuilder.WithParameter("projector", "Function to project the combined values.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"T{i}", $"Value type {i}.");
        }

        docBuilder.WithTypeParameter("TR", "Projected result type.");

        return new MethodWriter(
            "ZipAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            parameters.ToArray(),
            genericParams.ToArray(),
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }
}
