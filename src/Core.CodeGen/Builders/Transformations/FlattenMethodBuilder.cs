using UnambitiousFx.Core.CodeGen.Common;
using UnambitiousFx.Core.CodeGen.Design;

namespace UnambitiousFx.Core.CodeGen.Builders.Transformations;

/// <summary>
///     Builds Flatten extension methods for Result types.
/// </summary>
internal sealed class FlattenMethodBuilder
{
    private readonly string _baseNamespace;

    public FlattenMethodBuilder(string baseNamespace)
    {
        _baseNamespace = baseNamespace ?? throw new ArgumentNullException(nameof(baseNamespace));
    }

    /// <summary>
    ///     Builds a standalone Flatten method for a specific arity.
    /// </summary>
    public MethodWriter BuildStandaloneMethod(ushort arity)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var innerResultType = $"Result<{valueTypes}>";
        var outerResultType = $"Result<{innerResultType}>";
        var returnType = innerResultType;

        // Build generic parameters
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");

        var body = "return result.Bind(inner => inner);";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Flattens a nested Result, removing one level of nesting.")
                                            .WithParameter("result", "The nested result instance.")
                                            .WithReturns("The inner result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "Flatten",
            returnType,
            body,
            Visibility.Public,
            MethodModifier.Static,
            [
                new MethodParameter($"this {outerResultType}", "result")
            ],
            genericParams,
            docBuilder.Build()
        );
    }

    /// <summary>
    ///     Builds async Flatten method for Task/ValueTask Result types.
    /// </summary>
    public MethodWriter BuildAsyncMethod(ushort arity,
                                         bool isValueTask)
    {
        var valueTypes = GenericTypeHelper.BuildGenericTypeString(arity, "TValue");
        var innerResultType = $"Result<{valueTypes}>";
        var outerResultType = $"Result<{innerResultType}>";
        var asyncOuterType = isValueTask
                                 ? $"ValueTask<{outerResultType}>"
                                 : $"Task<{outerResultType}>";

        // Build generic parameters
        var genericParams = GenericTypeHelper.CreateGenericParameters(arity, "TValue", "notnull");

        var body = """
                   var outer = await awaitable;
                   return outer.Flatten();
                   """;

        var asyncReturn = isValueTask
                              ? $"ValueTask<{innerResultType}>"
                              : $"Task<{innerResultType}>";

        var docBuilder = DocumentationWriter.Create()
                                            .WithSummary("Async Flatten unwrapping a nested awaitable Result.")
                                            .WithParameter("awaitable", "The awaitable nested result instance.")
                                            .WithReturns("A task with the flattened result.");

        foreach (var i in Enumerable.Range(1, arity))
        {
            docBuilder.WithTypeParameter($"TValue{i}", $"Value type {i}.");
        }

        return new MethodWriter(
            "FlattenAsync",
            asyncReturn,
            body,
            Visibility.Public,
            MethodModifier.Async | MethodModifier.Static,
            [
                new MethodParameter($"this {asyncOuterType}", "awaitable")
            ],
            genericParams,
            usings: ["System", "System.Threading.Tasks", "UnambitiousFx.Core.Results.Extensions.Transformations"],
            documentation: docBuilder.Build()
        );
    }
}
